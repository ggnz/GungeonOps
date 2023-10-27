using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : Weapon_Script
{
    [Header("-Rifle")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    public int bulletDamage = 20;

    public int burstSize = 3; // Número de balas en cada ráfaga
    public float timeBetweenShots = 0.1f; // Tiempo entre cada disparo en la ráfaga

    private bool isShooting = false;

    public override void Shoot()
    {
        if (currentAmmo > 0 && !isShooting)
        {
            StopCoroutine("Reload");
            StartCoroutine(ShootBurst());
        }
    }

    private IEnumerator ShootBurst()
    {
        isShooting = true;

        for (int i = 0; i < burstSize; i++)
        {
            if (currentAmmo > 0)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Bullet_Script bulletScript = bullet.GetComponent<Bullet_Script>();

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

                yield return new WaitForSeconds(timeBetweenShots);
            }
        }

        isShooting = false;
    }
}
