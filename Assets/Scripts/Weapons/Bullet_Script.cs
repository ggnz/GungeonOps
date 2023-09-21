using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public Weapon_Script weapon_Script;
    public int damage;

    void Start() 
    {        
        weapon_Script = FindObjectOfType<Weapon_Script>();   
        damage = weapon_Script.bulletDamage;
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











