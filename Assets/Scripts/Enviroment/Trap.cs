using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public bool isActive = false;

    public ParticleSystem trapParticles;

   

    public IEnumerator ActivateTrap(float duration)
    {            
        yield return new WaitForSeconds(0.5f); 
        trapParticles.Play();
        isActive = true;
        //this.gameObject.SetActive(true);        
        yield return new WaitForSeconds(duration);        
        DesactivarTrampa();            
    }   
   
    public void DesactivarTrampa()
    {
        trapParticles.Stop();
        isActive = false;          
    }

    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy_Script>().IsDead();  

        }
        else if (isActive && other.CompareTag("Player"))
        {
            other.GetComponent<Character_Script>().takeDamage(50);
        }
        else
        {

        }
    }
}

