using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FastEnemy : Enemy_Script
{  
    public ParticleSystem dashParticles; 
    public ParticleSystem chargeParticles;   
    public float baseSpeed = 5f;

    [Header("-DASH")]
    public float dashSpeed = 15f;
    public float dashCD = 10f;

    bool canRun = true; // Variable para controlar el cooldown    

    new void Start()
    {
        base.Start();
        agente.speed = baseSpeed;
        // Llama a la función para que empiece el proceso de aumento de velocidad.
        StartCoroutine(TempSpeedBoostTimer());       
    }
    
    protected override void AttackPlayer()
    {
        isAttacking = true;
        if (player != null)
        {
            player.GetComponent<Character_Script>().takeDamage(attackDamage);
        }

        StartCoroutine(ResetAttackState());
    }

    IEnumerator TempSpeedBoost()
    {     
        if(canRun)
        {
            agente.speed = 1f;
            chargeParticles.Play(); 
            yield return new WaitForSeconds(1f); 

            agente.speed = dashSpeed;           
            chargeParticles.Stop(); 
            dashParticles.Play();   
            yield return new WaitForSeconds(3f);

           
            agente.speed = baseSpeed; // Restaura la velocidad normal
            canRun = false; // Desactiva temporalmente la habilidad de aumentar la velocidad
            dashParticles.Stop(); 
            yield return new WaitForSeconds(7f);

            canRun = true; // Reactiva la habilidad
           
            
        }
    }

    IEnumerator TempSpeedBoostTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashCD); // Espera 10 segundos antes de activar el aumento de velocidad
            StartCoroutine(TempSpeedBoost());
        }
    }
}
