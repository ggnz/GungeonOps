using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon_Script
{
    [Header("-Sniper ")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    public int bulletDamage = 20;
    public int bulletMaxHits = 1;

    private float force;
    private int damage;
    public int maxHits;

    public float shakeA = 0.05f;
    public float shakeD = 0.05f;
   
 
    public override void Charge(float chargeTime){
         
        
        if (chargeTime < 0.5f)
        {
            //main.startColor = new ParticleSystem.MinMaxGradient(Color1);
            force = bulletForce;
            damage = bulletDamage;
            maxHits = bulletMaxHits;
        }
        else if (chargeTime >= 0.5f && chargeTime < 2f)
        {
            //main.startColor = new ParticleSystem.MinMaxGradient(Color2);
            damage = Mathf.RoundToInt(bulletDamage * 1.5f);
            force = bulletForce * 1.5f;
            maxHits = 3;
        }
        else if (chargeTime >= 2f )
        {
            //main.startColor = new ParticleSystem.MinMaxGradient(Color3);
            damage = bulletDamage * 3;
            force = bulletForce * 3f;
            maxHits = 7;
            shakeA = 0.4f;
        }
        
        Shoot();
    }

    public override void Shoot()
    {
        Debug.Log(maxHits);
        if (currentAmmo > 0)
        {            
            cam.StartShake(shakeD, shakeA);
            
            StopCoroutine("Reload");

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            SniperBullet_Script bulletScript = bullet.GetComponent<SniperBullet_Script>();

            // Asigna el bulletDamage al script de la bala.
            if (bulletScript != null)
            {
                bulletScript.SetDamage(damage, maxHits);
                
            }

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * force, ForceMode2D.Impulse);

            currentAmmo--;
            force = bulletForce;
            damage = bulletDamage;
            maxHits = bulletMaxHits;
            shakeA = 0.05f;

            laserParticles.Stop();
            
            explosionParticles.Play();

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
            }
        }
}

}

