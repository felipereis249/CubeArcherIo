using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float dano;
    private float tempoFadeOut = 2.5f;
    private bool destruir = false;

    public float Dano { get => dano; set => dano = value; }
    public float TempoFadeOut { get => tempoFadeOut; set => tempoFadeOut = value; }
    public bool Destruir { get => destruir; set => destruir = value; }

    public Enemy()
    {
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CriaBarraDeVida()
    {

    }
}
