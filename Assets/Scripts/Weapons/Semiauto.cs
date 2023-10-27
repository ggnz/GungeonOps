using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semiauto : Weapon_Script
{
    [Header("-SemiAuto ")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    public int bulletDamage = 20;
    
    public float shakeA = 0.05f;
    public float shakeD = 0.05f;


    public override void Shoot()
    {
        if (currentAmmo > 0)
        {
            cam.StartShake(shakeD, shakeA);
            StopCoroutine("Reload");

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet_Script bulletScript = bullet.GetComponent<Bullet_Script>();

            // Asigna el bulletDamage al script de la bala.
            if (bulletScript != null)
            {
                bulletScript.SetDamage(bulletDamage);
            }

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

            currentAmmo--;

            explosionParticles.Play();

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }   

   
 
}

