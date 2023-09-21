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

    private void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();  
        IniciarRonda();
    }    

    private void Update(){
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            AumentarRonda();
        }
    }    

    public void IniciarRonda()
    {
        // Incrementa el número de ronda
        rondaActual++;
        hudManager.ShowRoundInfo(rondaActual);        

        // Reinicia el contador de enemigos generados
        spawnHandler.ReiniciarContadorEnemigos();
        
        StartCoroutine(TimeBetweenRounds());
    }

    private IEnumerator TimeBetweenRounds()
    {
        yield return new WaitForSeconds(timeBetweenRounds); // Espera 5 segundos

        // Después de esperar, inicia la generación de enemigos
        spawnHandler.IniciarGeneracion();
    }

    public void AumentarRonda()
    {
        IniciarRonda();         
        spawnHandler.maxEnemigosporRonda = Mathf.RoundToInt(5.5f * Mathf.Pow(1.1f, rondaActual));
        //Velocidad
        //Dano????????? pero necesitaria que el jugador tambien pueda subirse la vida
        
        if (rondaActual >= 9)
        {           
            aumentoVidaRonda = Mathf.RoundToInt(spawnHandler.vidaEnemigo * Mathf.Pow(1.1f, rondaActual - 1));
        }

        spawnHandler.vidaEnemigo += aumentoVidaRonda;
        
        
    }

    public void FinalizarRonda()
    {                
        spawnHandler.DetenerGeneracion();
        AumentarRonda();
        
    }
}

