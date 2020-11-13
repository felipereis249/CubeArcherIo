using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ovelha : Enemy
{
    [SerializeField] private Image barraVidaCheia;
    [SerializeField] private Image barraVidaVazia;
    [SerializeField] private Projetil flechaProjetil;

    private SpriteRenderer renderer;

    private Image enemyBarraVidaCheia;
    private Image enemyBarraVidaVazia;
    private float velocidade;
    private bool olhandoDireita;
    private float direcao;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        Destruir = false;
        TempoFadeOut = 1.0f;
        velocidade = Random.Range(1f, 4f);
        Dano = 0.01f;
        direcao = 1;
        olhandoDireita = true;

        enemyBarraVidaCheia = barraVidaCheia;
        enemyBarraVidaVazia = barraVidaVazia;
        enemyBarraVidaVazia.fillAmount = 1;
        enemyBarraVidaCheia.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Movimenta();
        AtualizaPosicoes();
        StatusVida();
        DestroiOvelha();
    }

    private void Movimenta()
    {
        // Movimenta o personagem no eixo X + velocidade
        if (!olhandoDireita)
            transform.Translate(new Vector3(1 * velocidade * Time.deltaTime * -1, 0, 0));
        else
            transform.Translate(new Vector3(1 * velocidade * Time.deltaTime, 0, 0));
    }

    private void AtualizaPosicoes()
    {        
        enemyBarraVidaVazia.rectTransform.position = new Vector3(this.transform.position.x + 0.2f, this.transform.position.y + 1.0f, 0);
        enemyBarraVidaCheia.rectTransform.position = new Vector3(this.transform.position.x + 0.2f, this.transform.position.y + 1.0f, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            // Decrementa a vida do player
            case "flecha":
                // Caso o objeto que atirou não seja o próprio player, decrementa a vida
                Projetil p = collision.gameObject.GetComponent<Projetil>();
                {
                    DecrementaVida(p.danoProjetil);
                    Debug.Log("Dano Projetil: " + p.danoProjetil.ToString()) ;
                }

                break;
            case "parede":
                AjustaDirecao(-1);
                break;
        }
    }

    private void AjustaDirecao(float dir)
    {
        transform.localScale = new Vector2(transform.localScale.x * dir, transform.localScale.y);

        olhandoDireita = olhandoDireita ? false : true;
    }
    private void DestroiOvelha()
    {
        if (Destruir)
        {
            TempoFadeOut -= Time.deltaTime;
            renderer.material.SetColor("_Color", new Color(255, renderer.color.g, renderer.color.b, TempoFadeOut));
        }
        if (TempoFadeOut <= 0)
        {
            // Destroi os Objetos da Ovelha
            GameOver();
        }
    }

    private void DecrementaVida(float dano)
    {
        enemyBarraVidaCheia.fillAmount -= dano;
    }
    private void StatusVida()
    {
        if (enemyBarraVidaCheia.fillAmount <= 0)
            Destruir = true;        
    }
    private void GameOver()
    {
        Destroy(this.gameObject);
        Destroy(enemyBarraVidaVazia.gameObject);
        Destroy(enemyBarraVidaCheia.gameObject);
    }
}
