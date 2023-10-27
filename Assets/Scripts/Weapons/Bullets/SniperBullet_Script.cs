using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet_Script : MonoBehaviour
{
    private int damage;
    private List<Enemy_Script> hitEnemies = new List<Enemy_Script>();
    public int maxHits; // Número máximo de enemigos que puede impactar la bala antes de destruirse

    // Método para configurar el daño de la bala.
    public void SetDamage(int damage, int maxHits)
    {
        this.damage = damage;
        this.maxHits = maxHits;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int noCollisionLayer = LayerMask.NameToLayer("NoCollision");

        if (collision.gameObject.layer != noCollisionLayer)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
                if (enemy != null && !hitEnemies.Contains(enemy))
                {
                    enemy.ReduceHealth(damage);
                    hitEnemies.Add(enemy);

                    if (hitEnemies.Count >= maxHits)
                    {
                        Destroy(gameObject); // Destruye la bala cuando alcanza el número máximo de impactos
                    }
                }
            }else{
                Destroy(gameObject);
            }
            
            
        }
    }
}

