using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemy : Enemy_Script
{  
    public float explosionRadius;
    public int explosionDamage;
    
    public ParticleSystem blastParticles;   


    protected override void AttackPlayer()
    {
        isAttacking = true;
        if (player != null)
        {
            player.GetComponent<Character_Script>().takeDamage(attackDamage);
        }

        StartCoroutine(ResetAttackState());
    } 

    public override void IsDead()
    {         
        score.SumaPuntos(puntosPorKill); 
        DropPowerUp();

        // Aquí añadimos la explosión al morir
        Explode();      
        
    }

    private void Explode()
    {
        
        gameObject.tag = "Untagged";
        // Detectar a los objetos en un radio y causarles daño
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<Enemy_Script>().ReduceHealth(explosionDamage);
            }
            
            else if (hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<Character_Script>().takeDamage(explosionDamage);
            }
        }

        // Efecto visual de explosión
        blastParticles.Play();
        StartCoroutine(Destruir());
    } 

    IEnumerator Destruir()
    {        
       yield return new WaitForSeconds(0.5f);
       Destroy(this.gameObject); 
       spawn.DisminuirEnemigosVivos();          
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    
    

}
