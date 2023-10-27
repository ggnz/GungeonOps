using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mono : Granade
{
    public Enemy_Script enemy;
    public Transform myPos;
   
    public float monkeyDuration = 7.5f;     
     
    public ParticleSystem danceParticles;  

    void Awake()
    {       
        Invoke("AttractEnemies", tiempoDeExplosion);          
    }

    void AttractEnemies()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.Sleep();
        trailParticles.Stop();

        danceParticles.Play();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Enemy_Script enemyScript = enemy.GetComponent<Enemy_Script>();
            enemyScript.ChangeTargetTemporarily(myPos, monkeyDuration);
        }

        StartCoroutine(ExplosionAfterDuration(monkeyDuration));
    }

    IEnumerator ExplosionAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        danceParticles.Stop();  
        cam.StartShake(shakeD, shakeA);
        explosionParticles.Play();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radioDeExplosion);
        foreach (Collider2D objetoCercano in colliders)
        {
            Enemy_Script enemyScript = objetoCercano.GetComponent<Enemy_Script>();
            if (enemyScript != null)
            {
                enemyScript.ReduceHealth(damage);
            }
        }
        StartCoroutine(DestruirGranada());
    }     
}

