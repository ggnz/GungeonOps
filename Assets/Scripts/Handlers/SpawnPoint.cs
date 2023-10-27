using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class SpawnPoint : MonoBehaviour
{
    public GameObject enemigoPrefab; // Prefab del enemigo
    public Barricade closestWindow;    

    public void GenerarEnemigo(int vidaEnemigo)
    {
        GameObject nuevoEnemigo = Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
        Enemy_Script enemyScript = nuevoEnemigo.GetComponent<Enemy_Script>();
        enemyScript.barricade = closestWindow;
        enemyScript.SetMaxHealth(vidaEnemigo); // Nuevo método para establecer la vida máxima del enemigo
    }
}*/



public class SpawnPoint : MonoBehaviour
{
    public GameObject[] enemigoPrefabs; // Array de prefabs de enemigos
    public Barricade closestWindow;    

    public void GenerarEnemigo(int vidaEnemigo)
    {
        int tipoEnemigo = Random.Range(0, enemigoPrefabs.Length); // Genera un número aleatorio entre 0 y el tamaño del arreglo - 1

        GameObject nuevoEnemigo = Instantiate(enemigoPrefabs[tipoEnemigo], transform.position, Quaternion.identity);
        Enemy_Script enemyScript = nuevoEnemigo.GetComponent<Enemy_Script>();
        enemyScript.barricade = closestWindow;
        enemyScript.SetMaxHealth(vidaEnemigo);
    }

}

