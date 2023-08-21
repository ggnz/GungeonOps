using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    //public float increasePerWave;


    // use a negative value for healing
    public void ReduceHealth(int damage)
    {
        currentHealth -= damage;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    /// Restores enemy to their max health for the current wave. Call this after creating an spawning.
    /*public void RestoreHealth(int wave)
    {
        currentHealth = (int)(maxHealth * increasePerWave * (wave + 1));
    }*/
}
