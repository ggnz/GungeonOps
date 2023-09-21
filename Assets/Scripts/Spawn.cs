using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{        
    public RoundHandler roundHandler;   
    

    public float tiempoInicial;
    public float tiempoEntreGeneraciones = 5f; // Tiempo entre generaciones de enemigos
    public GameObject[] spawns; // Array de Spawns    
     
    public int maxEnemigosporRonda = 6; // Máximo de enemigos generados en total por todos los spawns
    public int enemigosGenerados = 0; // Contador de enemigos generados
    
    public int enemigosVivos = 0;

    public int vidaEnemigo = 100;

    private void Start()
    {        
        InvokeRepeating("GenerarEnemigo", tiempoInicial, tiempoEntreGeneraciones);
    } 

    public void IniciarGeneracion()
    {
        InvokeRepeating("GenerarEnemigo", tiempoInicial, tiempoEntreGeneraciones);
    }      

    void GenerarEnemigo()
    {   
        // Detiene la generación de enemigos si se alcanza el máximo
        if (enemigosGenerados >= maxEnemigosporRonda)
        {
            CancelInvoke("GenerarEnemigo"); 
            return;
        }

        // Seleccionar un spawn aleatorio
        int indiceSpawnAleatorio = Random.Range(0, spawns.Length);
        GameObject spawnAleatorio = spawns[indiceSpawnAleatorio];
        
        spawnAleatorio.GetComponent<SpawnPoint>().GenerarEnemigo(vidaEnemigo); // Llama a la función de generación de enemigos en el spawn seleccionado
        
        enemigosGenerados++;

        AumentarEnemigosVivos();
    }
 

    public void ReiniciarContadorEnemigos()
    {
        enemigosGenerados = 0;
    }    

    public void DetenerGeneracion()
    {
        CancelInvoke("GenerarEnemigo");
    }


    public void AumentarEnemigosVivos()
    {
        enemigosVivos++;
    }

    public void DisminuirEnemigosVivos()
    {
        enemigosVivos--;
        if(enemigosVivos <= 0){            
            roundHandler.FinalizarRonda();
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