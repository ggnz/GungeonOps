using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour
{
    public Spawn spawnHandler; // Referencia al SpawnHandler
    public HUDManager hudManager;

    public float timeBetweenRounds = 5f;

    private int rondaActual = 0;    

    [Header("-Enemy Behavior")]
    public int aumentoVidaRonda =  100;
    public int maxEnemigosporRonda = 6;
    public int vidaEnemigo = 100;

    private void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();  
        rondaActual++;
        hudManager.ShowRoundInfo(rondaActual);     
        StartCoroutine(TimeBetweenRounds());
    }    

    private void Update(){
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            FinalizarRonda();
        }
    }    

    public void IniciarRonda()
    {                      
        spawnHandler.IniciarGeneracion(maxEnemigosporRonda, vidaEnemigo);
    }

    private IEnumerator TimeBetweenRounds()
    {
        yield return new WaitForSeconds(timeBetweenRounds); // Espera 5 segundos

        // Después de esperar, inicia la generación de enemigos
        IniciarRonda();
    }   

    public void FinalizarRonda()
    {                
        spawnHandler.DetenerGeneracion();
        maxEnemigosporRonda = Mathf.RoundToInt(5.5f * Mathf.Pow(1.1f, rondaActual));
        //Velocidad
        //Dano????????? pero necesitaria que el jugador tambien pueda subirse la vida
        
        /*
        if (rondaActual >= 9)
        {           
            aumentoVidaRonda = Mathf.RoundToInt(vidaEnemigo * Mathf.Pow(1.1f, rondaActual - 1));
        }*/

        vidaEnemigo += aumentoVidaRonda;
        rondaActual++;
        hudManager.ShowRoundInfo(rondaActual);   
        StartCoroutine(TimeBetweenRounds());
        
    }
}

