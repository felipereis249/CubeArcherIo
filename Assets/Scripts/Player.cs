using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D player;
    [SerializeField] private Image arco;
    [SerializeField] private Projetil flechaProjetil;
    [SerializeField] private GameObject pontaFlecha;
    [SerializeField] private Image barraForcaVazia;
    [SerializeField] private Image barraForcaCheia;
    [SerializeField] private Image barraVidaVazia;
    [SerializeField] private Image barraVidaCheia;
    
    private bool olhandoDireita;
    private bool jaAtirou;
    public float sensibilidadeMira = 1f;
    public float inverterEixo = 1;
    private float zRotate = 2.5f;
    public float velocidade = 6.0f;
    public float forca = 70f;
    private float forcaProjetil = 30f;
    public bool liberaPulo;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        olhandoDireita = true;
        liberaPulo = true;
        jaAtirou = false;
        barraForcaCheia.fillAmount = 0;
        
        // Informacoes do Projetil
        flechaProjetil.Owner = this.gameObject;
    }

    void Update()
    {
        Move();
        InputActions();
        Correcoes();
        StatusVida();
    }

    /// <summary>
    /// Movimenta o personagem
    /// </summary>
    private void Move()
    {
        float H = Input.GetAxis("Horizontal");

        // Altera a direção do personagem de acordo com a tecla pressionada (direita ou esquerda, eixo Axis Horizontal)
        AjustaDirecao(H);

        // Movimenta o personagem no eixo + aceleração
        transform.Translate(new Vector3(H * velocidade * Time.deltaTime, 0, 0));
        
        // Movimenta o arco junto com o personagem
        arco.rectTransform.position = this.transform.position;

        // Movimenta a barra de vida e de força
        barraForcaVazia.rectTransform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.1f, this.transform.position.z);
        barraForcaCheia.rectTransform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.1f, this.transform.position.z);
        barraVidaVazia.rectTransform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, 0);
        barraVidaCheia.rectTransform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.8f, 0);
    }

    /// <summary>
    /// Gerencia as ações do jogador
    /// </summary>
    private void InputActions()
    {
        // Verifica se o player pressionou a tecla SPACE para efetuar um salto
        if (Input.GetKeyDown(KeyCode.Space) && liberaPulo)
        {
            player.AddForce(new Vector2(0, forca * Time.deltaTime), ForceMode2D.Impulse);
            liberaPulo = false;
        }

        // Rotaciona a flecha do personagem baseado na posição Y do mouse
        if (Input.GetMouseButton(0))
        {
            float mouseY = Input.GetAxis("Mouse Y");
            float dir = 1f;
            if (!olhandoDireita) dir = -1f;
            if (mouseY > 0)
            {
                RotacionaFlecha((-sensibilidadeMira * inverterEixo)/2 * dir);
            }
            else if (mouseY < 0)
            {
                RotacionaFlecha((sensibilidadeMira * inverterEixo)/2 * dir);
            }

            float mouseX = Input.GetAxis("Mouse X");
            if (mouseX < 0)
            {
                barraForcaCheia.fillAmount += 1.5f * Time.deltaTime * dir;
                forcaProjetil = barraForcaCheia.fillAmount * 100;
            }
            if (mouseX > 0)
            {
                barraForcaCheia.fillAmount -= 1.5f * Time.deltaTime * dir;
                forcaProjetil = barraForcaCheia.fillAmount * 100;
            }
        }

        // Verifica se está disparando
        //bool isFiring = Input.GetAxis("Fire1") == 1 ? true : false;
        bool isFiring = Input.GetMouseButtonUp(0);        
        if (isFiring)
        {
            if (!jaAtirou)
            {
                // Ajusta a direção do projétil
                float direcao = 1;
                if (!olhandoDireita) direcao = -1;

                // Cria um GameObject da flecha e adiciona a força
                //Rigidbody2D f = Instantiate(flechaProjetil.GetComponent<Rigidbody2D>(), new Vector3(pontaFlecha.transform.position.x, pontaFlecha.transform.position.y, 0), pontaFlecha.transform.rotation);
                Vector3 vec = new Vector3(pontaFlecha.transform.position.x, pontaFlecha.transform.position.y, pontaFlecha.transform.rotation.z);
                Projetil proj = Instantiate(flechaProjetil, vec, pontaFlecha.transform.rotation);
                Rigidbody2D rb2dProj = proj.GetComponent<Rigidbody2D>();

                // Adiciona a referência do objeto PLAYER
                proj.Owner = this.gameObject;
                proj.danoProjetil = 0.5f;

                // Ajusta a direção do projétil
                rb2dProj.transform.localScale = new Vector2(proj.transform.localScale.x * direcao, proj.transform.localScale.y);

                // Calcula e aplica a força angular
                float flechaX = (forcaProjetil) * Mathf.Cos(zRotate * Mathf.Deg2Rad) * direcao;
                float flechaY = (forcaProjetil) * Mathf.Sin(zRotate * Mathf.Deg2Rad) * direcao;

                rb2dProj.AddForce(new Vector2(flechaX, flechaY));

                // Reseta as variáveis de controle de força e estados
                jaAtirou = true;
                barraForcaCheia.fillAmount = 0;
                forcaProjetil = 30f;
            }
        }
        else
        {
            jaAtirou = false;
        }
    }

    // Modifica o ângulo do arco no eixo Z
    private void RotacionaFlecha(float rotacaoZ)
    {
        zRotate += rotacaoZ;
        arco.rectTransform.eulerAngles = new Vector3(0, 0, zRotate);
    }

    /// <summary>
    /// Altera a direção do personagem no eixo Horizontal
    /// </summary>
    /// <param name="eixoH"></param>
    private void AjustaDirecao(float eixoH)
    {
        // Alterna a direção dos objetos
        if ((olhandoDireita && eixoH < 0))
        {
            olhandoDireita = false;
            AlteraDirecaoObj(-1f);
        }
        else if (!olhandoDireita && eixoH > 0)
        {
            olhandoDireita = true;
            AlteraDirecaoObj(-1f);
        }
    }
    private void AlteraDirecaoObj(float direcaoObjH)
    {
        transform.localScale = new Vector2(transform.localScale.x * direcaoObjH, transform.localScale.y);
        arco.transform.localScale = new Vector2(arco.transform.localScale.x * direcaoObjH, arco.transform.localScale.y);
        zRotate = zRotate * -1;
        RotacionaFlecha(0);
        pontaFlecha.transform.localScale = new Vector2(pontaFlecha.transform.localScale.x * direcaoObjH, pontaFlecha.transform.localScale.y);
    }

    private void DecrementaVida(float dano)
    {
        barraVidaCheia.fillAmount -= dano;
    }
    private void StatusVida()
    {
        if (barraVidaCheia.fillAmount <= 0)
            GameOver();
    }
    private void GameOver()
    {
        Destroy(this.gameObject);
        Destroy(arco.gameObject);
        Destroy(barraVidaCheia.gameObject);
        Destroy(barraVidaVazia.gameObject);
        Destroy(barraForcaCheia.gameObject);
        Destroy(barraForcaVazia.gameObject);
    }

    #region CORREÇÕES
    private void Correcoes()
    {
        // Correções dos ângulos máximos da flecha
        if (zRotate > 60)
        {
            zRotate = 60;
        }
        if (zRotate < -60)
        {
            zRotate = -60;
        }

    }
    #endregion

    #region EVENTOS DE COLISÃO
    /// <summary>
    /// Tratamento das colisões do Player
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;        

        switch (tag) {
            // Verifica se tocou o chão e libera o pulo
            case "chao": liberaPulo = true; break;

            // Decrementa a vida do player
            case "flecha":
                // Caso o objeto que atirou não seja o próprio player, decrementa a vida
                Projetil p = collision.gameObject.GetComponent<Projetil>();
                if (p != null && p.Owner != this.gameObject)
                    DecrementaVida(p.danoProjetil);
                break;
            case "ovelha_turbo":
                Ovelha ov = collision.gameObject.GetComponent<Ovelha>();
                DecrementaVida(ov.Dano);
                break;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case "ovelha_turbo":
                Ovelha ov = collision.gameObject.GetComponent<Ovelha>();
                DecrementaVida(ov.Dano);
                break;
        }
    }
    #endregion
}
