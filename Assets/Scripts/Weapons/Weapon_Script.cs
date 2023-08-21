using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Script : MonoBehaviour
{    
    public static Weapon_Script Instance;
    Coroutine currentCoroutine;

    [Header("-INFO")]
    public int currentAmmo;
    public bool isReloading = false;

    [Header("-RELOAD")]
    public float reloadTime = 2f;
    private ReloadTimer reloadTimer;

    [Header("-BULLETS")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletForce = 20f;
    public int bulletDamage;

    [Header("-AMMO")]
    
    public int ammoMagazine = 8;
    public int ammoStock = 30;

    [Header("-FIRE RATE")]
    public float fireRate;

    void Start()
    {
        currentAmmo = ammoMagazine;

        Instance = this;
    }       


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0) && currentAmmo > 0 && isReloading == false)
        {
            if(currentCoroutine == null)
            currentCoroutine = StartCoroutine(Shoot());
        }     

        if (Input.GetKey("r") && isReloading == false)
        {
            StartCoroutine(Reload());
            return;
        }
        
    }
    /*IEnumerator DoShoot() {
        Shoot();
        yield return new WaitForSeconds(fireRate);
        currentCoroutine = null;
    }*/

    IEnumerator Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);       
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);

        currentAmmo--;     
        if (currentAmmo <= 0 )
        {
            StartCoroutine(Reload());            
        }     
        yield return new WaitForSeconds(fireRate);
        currentCoroutine = null;    
    }

    IEnumerator Reload()
    {
        int reloadAmmo = ammoMagazine - currentAmmo;

        Debug.Log(ammoMagazine + "/" + ammoStock);  
        isReloading = true;
        ReloadTimer.Instance.isCoolingDown = true;
        Debug.Log("Reloading...");        

        if(ammoStock >= ammoMagazine) 
        {//if there's enough ammo in the stock for reloading
            currentAmmo += reloadAmmo;
            ammoStock -= reloadAmmo;
        }else
        {//if there's NO enough ammo in the stock for reloading
            currentAmmo = ammoStock;
            ammoStock -= currentAmmo;
            Debug.Log("fghfhfgh");
        }
        
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;

        ReloadTimer.Instance.isCoolingDown = false;
        //ReloadTimer.Instance.filled.fillAmount = 0.0f;
     
        
    }
}




//how to grab a wapon from the floor in unity?




//Source: https://stackoverflow.com/questions/73008973










