using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommonEnemy : Enemy_Script
{  
    protected override void AttackPlayer()
    {
        isAttacking = true;
        if (player != null)
        {
            player.GetComponent<Character_Script>().takeDamage(attackDamage);
        }

        StartCoroutine(ResetAttackState());
    }
    
    
    

}
