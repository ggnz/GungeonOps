using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5.0f;
    public int damage = 10;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Character_Script>().takeDamage(damage);
            Destroy(gameObject);
           
        }else if (other.CompareTag("Wall")){
            Destroy(gameObject);
        }
       
    }
}
