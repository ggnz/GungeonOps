using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHandler : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;
    public float probabilityToSpawnPowerUp = 0.02f;
    public Score score;
    public HUDManager hudManager;

    public bool isInsta = false;


    
    // Start is called before the first frame update
   
    public void Start(){
        hudManager = FindObjectOfType<HUDManager>(); 
        score = FindObjectOfType<Score>();        
    }

    public void SpawnRandomPowerUp(Vector3 position)
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= probabilityToSpawnPowerUp)
        {
            // Seleccionar un power-up al azar
            GameObject randomPowerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
            
            // Instanciar el power-up en la posición especificada
            Instantiate(randomPowerUp, position, Quaternion.identity);
        }
    }

    public Coroutine activeInstakillCoroutine;
    public IEnumerator ActiveInstakill(Enemy_Script enemy, float duration, Sprite logo)
    {             
        isInsta = true;         
        yield return new WaitForSeconds(duration);             
  
        hudManager.HidePowerUpLogo(logo); 
        isInsta = false;   

        activeInstakillCoroutine = null; // Limpiar la referencia
        
    } 
  
    public Coroutine activeDoublePointsCoroutine;
    public IEnumerator ActiveDoublePoints(float duration, Sprite logo)
    {     
        score.areDouble = true;
        yield return new WaitForSeconds(duration);        
        
        hudManager.HidePowerUpLogo(logo);  
        score.areDouble = false;

        activeDoublePointsCoroutine = null; // Limpiar la referencia
    }
    
    
}




