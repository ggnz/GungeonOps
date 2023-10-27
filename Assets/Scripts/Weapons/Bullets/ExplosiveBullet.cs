using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    public int damage = 20; // Daño de la explosión
    public float explosionRadius = 5f; // Radio de la explosión
    public ParticleSystem explosionParticles;
    public ParticleSystem trailParticles;

    public float shakeA = 0.05f;
    public float shakeD = 0.05f;


    void Awake(){
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int noCollisionLayer = LayerMask.NameToLayer("NoCollision");

        if (collision.gameObject.layer != noCollisionLayer)
        {
            explosionParticles.Play();
            Explode();        
            // Detener movimiento
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            trailParticles.Stop();
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;    
        }
    }

    private void Explode()
    {
        Camera_Script cam = FindObjectOfType<Camera_Script>();
        cam.StartShake(shakeD, shakeA);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hitCollider in colliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                Enemy_Script enemy = hitCollider.GetComponent<Enemy_Script>();
                enemy?.ReduceHealth(damage);
            }
        }

        // Destruye la bala después de la explosión
        StartCoroutine(DestruirBullet());
       
    }

    IEnumerator DestruirBullet()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
