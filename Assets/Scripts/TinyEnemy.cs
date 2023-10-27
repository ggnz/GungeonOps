using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TinyEnemy : Enemy_Script
{
    private bool isQTEActive = false;
    public int requiredKeyPresses = 10; // Número de veces que se debe presionar la tecla "E"


    protected override void AttackPlayer()
    {
        isAttacking = true;
        if (player != null)
        {
            player.GetComponent<Character_Script>().takeDamage(attackDamage);
        }

        StartCoroutine(ResetAttackState());
    }

    new private void Update()
    {
        base.Update();
        if (isQTEActive)
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                ReduceHealth(maxHealth / requiredKeyPresses);
            }
        }
    }

 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            transform.SetParent(collision.transform);
            Collider2D collider = GetComponent<Collider2D>();
            collider.isTrigger = true;

            // Habilita el QTE
            isQTEActive = true;
        }
    }

    
}
