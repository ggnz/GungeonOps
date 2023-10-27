using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
    public float attackDelay = 0.5f;
    private float lastAttackTime = 0f;
    
    public int damage = 100;

    public EdgeCollider2D swordCollider;
    

    [Header("-Slash Particles")] 
    public ParticleSystem slashParticles;     

    public override void PerformAttack()
    {
        float currentTime = Time.time;
        if (currentTime - lastAttackTime < attackDelay)
        {
            // Aún no ha pasado suficiente tiempo desde el último ataque, no hacer nada
            return;
        }

        // Se puede realizar un ataque
        lastAttackTime = currentTime;

        // Resto del código del ataque
        slashParticles.Stop();
        slashParticles.Play();       
        
        
        Collider2D[] hits = new Collider2D[10]; // Ajusta el tamaño según tus necesidades

        int numHits = swordCollider.OverlapCollider(new ContactFilter2D(), hits);

        for (int i = 0; i < numHits; i++)
        {
            if (hits[i].CompareTag("Enemy"))
            {
                Enemy_Script enemy = hits[i].GetComponent<Enemy_Script>();
                if (enemy != null)
                {
                    enemy.ReduceHealth(damage);
                }
            }
        }
    }
}

