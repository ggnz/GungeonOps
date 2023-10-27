using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky: Granade
{        
    private bool explotada = false;
    public ParticleSystem countdownParticles;    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {         
            countdownParticles.Play(); 

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.SetParent(collision.transform);
            
            Invoke("Explotar", tiempoDeExplosion);            
        }
    }
  
    void Explotar()
    {
        if (!explotada)
        {
            cam.StartShake(shakeD, shakeA);
            countdownParticles.Stop();
            explotada = true;
            transform.SetParent(null); // Desvincula la granada
            explosionParticles.Play();          

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

