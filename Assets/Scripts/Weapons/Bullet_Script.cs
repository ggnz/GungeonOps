using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public static Bullet_Script bullet_Script;
    public int damage;

    void Start() 
    {
        
        damage = Weapon_Script.Instance.bulletDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);      
        if(collision.gameObject.CompareTag("Enemy")){
            Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
            if (enemy)
            {
                enemy.ReduceHealth(damage);

                if (enemy.IsDead())
                {                    
                    Destroy(collision.gameObject);
                }
            }
        }
        
    }
}











