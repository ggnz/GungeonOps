using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frag : Granade
{
    private bool explotada = false;    
 

    void Awake()
    {
        Invoke("Explotar", tiempoDeExplosion);     
        
    }  

    void Explotar()
    {
        if (!explotada)
        {
            cam.StartShake(shakeD, shakeA);
            trailParticles.Stop();
            explotada = true;
            explosionParticles.Play();

            // Aplicar fuerza a objetos cercanos
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radioDeExplosion);
            foreach (Collider2D objetoCercano in colliders)
            {
                if (objetoCercano.CompareTag("Enemy"))
                {                    
                    Enemy_Script enemyScript = objetoCercano.GetComponent<Enemy_Script>();
                    if (enemyScript != null)
                    {
                        enemyScript.ReduceHealth(damage);
                    }
                }
            }            
            StartCoroutine(DestruirGranada());
        }
    }    
}
