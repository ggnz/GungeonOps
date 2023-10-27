using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon_Script
{
    [Header("-Escopeta")]
    public int bulletDamage = 20; // Daño de cada disparo
    public PolygonCollider2D coneCollider;
    public float shakeA = 0.05f;
    public float shakeD = 0.05f;

    public override void Shoot()
    {
        if (currentAmmo > 0)
        {
            cam.StartShake(shakeD, shakeA);
            StopCoroutine("Reload");

            // Define los parámetros del Raycast
            
            Collider2D[] hits = new Collider2D[10]; // Ajusta el tamaño según tus necesidades

            int numHits = coneCollider.OverlapCollider(new ContactFilter2D(), hits);

            for (int i = 0; i < numHits; i++)
            {
                if (hits[i].CompareTag("Enemy"))
                {
                    Enemy_Script enemy = hits[i].GetComponent<Enemy_Script>();
                    if (enemy != null)
                    {
                        enemy.ReduceHealth(bulletDamage);
                    }
                }
            }
            

            currentAmmo--;

            explosionParticles.Play();

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }
 

}

