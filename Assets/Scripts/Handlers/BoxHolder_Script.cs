using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxHolder_Script : MonoBehaviour
{
    public GameObject SpawnBody;
    public GameObject Box;
    public GameObject activeSpawn;
    public GameObject nextSpawn;
    public GameObject[] boxSpawns;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn Inicial
        int spawnSelector = Random.Range(0, boxSpawns.Length);         
        activeSpawn = boxSpawns[spawnSelector].gameObject;
   
        Box = activeSpawn.transform.GetChild(0).gameObject;
        SpawnBody = activeSpawn.transform.GetChild(1).gameObject;
        
        activaCajaNueva();
    }

    public void cambiaLocalizacion(){        
        //cambia localizacion        
        desactivaCajaAnterior();        
        
        //Escoje aleatoriamente el nextSpawn excluyendo el activeSpawn
        int spawnSelector;
        do{            
            spawnSelector = Random.Range(0, boxSpawns.Length);
        } while (boxSpawns[spawnSelector].gameObject == activeSpawn);        
        
        //Define el siguiente spawn
        nextSpawn = boxSpawns[spawnSelector].gameObject;
        Box = nextSpawn.transform.GetChild(0).gameObject;
        SpawnBody = nextSpawn.transform.GetChild(1).gameObject;
        
        activaCajaNueva();   
          
        // Actualiza activeSpawn para reflejar el nuevo objeto activo
        activeSpawn = nextSpawn;    
          
    }

    public void desactivaCajaAnterior(){
        SpawnBody.SetActive(true);
        Box.SetActive(false);
    }

    public void activaCajaNueva(){
        Box.SetActive(true);
        SpawnBody.SetActive(false);
    }

    
}
