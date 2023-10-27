using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    private int damage;

    // Método para configurar el daño de la bala.
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {              
        int noCollisionLayer = LayerMask.NameToLayer("NoCollision");

        if (collision.gameObject.layer != noCollisionLayer) {
            Destroy(gameObject);
            
            if (collision.gameObject.CompareTag("Enemy")) {
                Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
                enemy?.ReduceHealth(damage);
            }
        }
                    
    }
}











