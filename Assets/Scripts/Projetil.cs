using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projetil : MonoBehaviour
{

    private float vidaProjetil = 2.0f;
    public float danoProjetil = 0.5f;
    private SpriteRenderer renderer;
    private bool destruir;
    private GameObject owner;

    // Referência do objeto pai
    public GameObject Owner { get => owner; set => owner = value; }
    
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        destruir = false;
        danoProjetil = 0.5f;        
    }

    // Update is called once per frame
    void Update()
    {
        DestroiProjetil();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao") && vidaProjetil > 0)
        {
            destruir = true;
        }
        if (collision.gameObject.CompareTag("ovelha_turbo") && vidaProjetil > 0)
        {
            vidaProjetil = 0.2f;
            destruir = true;
        }

    }

    private void DestroiProjetil()
    {
        if (destruir)
        {
            vidaProjetil -= Time.deltaTime;
            renderer.material.SetColor("_Color", new Color(renderer.color.r, renderer.color.g, renderer.color.b, vidaProjetil));
        }
        if (vidaProjetil <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
