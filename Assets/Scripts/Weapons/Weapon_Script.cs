using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Script : MonoBehaviour
{    
    public static Weapon_Script Instance;
    Coroutine currentCoroutine;
    public HUDManager hudManager;
    public Character_Script player;  


    [Header("-INFO")]
    public int currentAmmo;       //privado
    public int currentAmmoStock;  //privado
    public string nombreArma;
    public string costoArma;
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
    public int ammoMagazine;
    public int ammoStock;
    public bool isFullAmmo;

    [Header("-FIRE RATE")]
    public float fireRate;
    private float lastShotTime = 0f;

    void Start()
    {
        player = FindObjectOfType<Character_Script>();      

        nombreArma = this.gameObject.name; 

        hudManager = FindObjectOfType<HUDManager>();
        
        currentAmmo = ammoMagazine;
        currentAmmoStock = ammoStock;

        Instance = this;
    }       


    // Update is called once per frame
    void Update()
    {
        hudManager.UpdateAmmo(currentAmmo, currentAmmoStock, nombreArma);
        
        if (Input.GetMouseButton(0) && currentAmmo > 0 && isReloading == false)
        {
            if (Time.time - lastShotTime >= fireRate)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }   

        if (Input.GetKey("r") && isReloading == false)
        {
            StartCoroutine(Reload());            
            return;
        }  

        if (Input.GetKey("0") )
        {
            FillAmmo();
        }  
        
        
    }

    /////////
    public void FillAmmo()
    {        
        Debug.Log("Recargando munición completa...");
        currentAmmo = ammoMagazine;
        currentAmmoStock = ammoStock;
        Debug.Log("Munición recargada: " + currentAmmo);
        
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);       
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);        

            currentAmmo--;          
            if (currentAmmo <= 0 )
            {
                StartCoroutine(Reload());            
            }
        }
    }

    IEnumerator Reload()
    {           

        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        
        int reloadAmmo = ammoMagazine - currentAmmo;        
        
        ReloadTimer.Instance.isCoolingDown = true;
        Debug.Log("Reloading...");        

        if(currentAmmoStock >= ammoMagazine) 
        {//if there's enough ammo in the stock for reloading
            currentAmmo += reloadAmmo;
            currentAmmoStock -= reloadAmmo;
        }else
        {//if there's NO enough ammo in the stock for reloading
            currentAmmo = currentAmmoStock;
            currentAmmoStock -= currentAmmo;            
        }               

        ReloadTimer.Instance.isCoolingDown = false;        
        
    }
}












