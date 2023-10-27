using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy_Script
{  
    public GameObject projectilePrefab;
    public float force = 20f;
    public Transform shootingPoint;

    public ParticleSystem shootParticles; 

    protected override void AttackPlayer()
    {
        shootParticles.Play();
        isAttacking = true;
    
        if (player != null)
        {
            Vector2 direction = (player.transform.position - shootingPoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().damage = attackDamage;
            
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * (force/2);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }

        StartCoroutine(ResetAttackState());
    }
    
    
    

}
