using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{        
    public RoundHandler roundHandler;   
    

    public float tiempoInicial;
    public float tiempoEntreGeneraciones = 5f; // Tiempo entre generaciones de enemigos
    public GameObject[] spawns; // Array de Spawns    
     
    public int maxEnemigos; // Máximo de enemigos generados en total por todos los spawns
    public int enemigosGenerados = 0; // Contador de enemigos generados
    
    public int enemigosVivos = 0; 
    public int enemigosEliminados = 0;
    private int vida = 100;

    void Update(){
        if(Input.GetKey("1")){
            tiempoInicial = 777f;
            tiempoEntreGeneraciones = 777f;
            roundHandler.FinalizarRonda();
        }        
    }    

    public void IniciarGeneracion(int maxEnemigosporRonda, int vidaEnemigo)
    {        
        maxEnemigos = maxEnemigosporRonda ;
        vida = vidaEnemigo;
        InvokeRepeating("GenerarEnemigo", tiempoInicial, tiempoEntreGeneraciones);
    }      

    void GenerarEnemigo()
    {   
        // Detiene la generación de enemigos si se alcanza el máximo
        if (enemigosGenerados >= maxEnemigos)
        {
            CancelInvoke("GenerarEnemigo"); 
            return;
        }

        // Seleccionar un spawn aleatorio
        int indiceSpawnAleatorio = Random.Range(0, spawns.Length);
        GameObject spawnAleatorio = spawns[indiceSpawnAleatorio];
        
        spawnAleatorio.GetComponent<SpawnPoint>().GenerarEnemigo(vida); // Llama a la función de generación de enemigos en el spawn seleccionado
        
        enemigosGenerados++;
        enemigosVivos++;      
    }  

    public void DetenerGeneracion()
    {
        CancelInvoke("GenerarEnemigo");
    } 

    public void DisminuirEnemigosVivos()
    {
        enemigosVivos--;
        enemigosEliminados++;
        if(enemigosVivos <= 0 && enemigosGenerados >= maxEnemigos){            
            roundHandler.FinalizarRonda();
            enemigosGenerados = 0;
            enemigosVivos = 0;
        }
    }

    public void HabilitarNuevosSpawns(Transform[] nuevosSpawns)
    {
        // Crear un nuevo array que tenga el tamaño de los spawns actuales más los nuevos spawns
        GameObject[] nuevosSpawnsArray = new GameObject[spawns.Length + nuevosSpawns.Length];

        // Copiar los spawns actuales al nuevo array
        spawns.CopyTo(nuevosSpawnsArray, 0);

        // Agregar los nuevos spawns al final del array
        for (int i = 0; i < nuevosSpawns.Length; i++)
        {
            nuevosSpawnsArray[spawns.Length + i] = nuevosSpawns[i].gameObject;
        }

        // Asignar el nuevo array de spawns
        spawns = nuevosSpawnsArray;
    }




    
}