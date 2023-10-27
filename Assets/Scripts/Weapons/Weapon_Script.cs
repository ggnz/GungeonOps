
using System.Drawing;
using System.Collections;
using UnityEngine;

public class Weapon_Script : MonoBehaviour
{
    [Header("-Weapon")]
    public ParticleSystem explosionParticles;
    public ParticleSystem laserParticles;

    [Header("-Laser Colors")]
    public UnityEngine.Color Color1;
    public UnityEngine.Color Color2;
    public UnityEngine.Color Color3;

    public HUDManager hudManager;
    public Character_Script player;
    public Camera_Script cam;
    public WeaponHolder_Script weaponHolder;
    

    public int currentAmmo;
    public int currentAmmoStock;
    public bool isReloading = false;

    public float reloadTime = 2f;


    public bool isFullAmmo;

    
    private float lastShotTime = 0f;
    private bool isCharging = true;

    public float chargeTime;   

    [Header("-Weapon Stats")]
    public string weaponName;
    public string weaponCost;
    public enum Modes
    {
        Auto,
        SemiAuto,
        Charge,
        Burst
        
    }
    public Modes FireMode;
    public float fireRate;
    public int ammoMagazine;
    public int ammoStock;       
   
    
    public virtual void Start()
    {
        weaponHolder = FindObjectOfType<WeaponHolder_Script>();
        player = FindObjectOfType<Character_Script>();
        cam = FindObjectOfType<Camera_Script>();
        weaponName = this.gameObject.name;
        hudManager = FindObjectOfType<HUDManager>();
        currentAmmo = ammoMagazine;
        currentAmmoStock = ammoStock;
        isReloading = false;
        
    }
    

    public virtual void Update()
    {
        hudManager.UpdateAmmo(currentAmmo, currentAmmoStock, weaponName);

        if (FireMode == Modes.Auto)
        {
            if (Input.GetButton("Fire1")  && currentAmmo > 0 && !isReloading)
            {
                if (Time.time - lastShotTime >= fireRate)
                {
                    Shoot();
                    lastShotTime = Time.time;
                }
            }
        }
        else if (FireMode == Modes.SemiAuto)
        {
            if (Input.GetButtonDown("Fire1")  && currentAmmo > 0 && !isReloading)
            {
                if (Time.time - lastShotTime >= fireRate)
                {
                    Shoot();
                    lastShotTime = Time.time;
                }
            }
        }
        else if (FireMode == Modes.Charge)
        {
            
            var main = laserParticles.main;
            if (Input.GetButton("Fire1")  && currentAmmo > 0 && !isReloading)
            {                 
                if (Time.time - lastShotTime >= fireRate)
                {         
                                     
                    isCharging = true;
                    laserParticles.Play();
                    chargeTime += Time.deltaTime;
                    if (chargeTime < 0.5f)
                    {
                        main.startColor = new ParticleSystem.MinMaxGradient(Color1);
                    }
                    else if (chargeTime >= 0.5f && chargeTime < 2f)
                    {
                        main.startColor = new ParticleSystem.MinMaxGradient(Color2);
                    }
                    else if (chargeTime >= 2f )
                    {
                        main.startColor = new ParticleSystem.MinMaxGradient(Color3);
                    }
                }
            }
            else if ((Input.GetButtonUp("Fire1")))
            {
                if (isCharging)
                {
                    Charge(chargeTime);      
                    lastShotTime = Time.time;
                }
                isCharging = false;
                chargeTime = 0f;
            }
        }
        else if (FireMode == Modes.Burst){
            if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && !isReloading)
            {
                if (Time.time - lastShotTime >= fireRate)
                {
                    Shoot();
                    lastShotTime = Time.time;
                }
            }
        }


        if ((Input.GetKeyDown("r") || Input.GetButtonUp("Reload")) && !isReloading && currentAmmoStock > 0 && currentAmmo < ammoMagazine)
        {
            StartCoroutine(Reload());
        }

        
        
    }


    public virtual void FillAmmo()
    {        
        currentAmmo = ammoMagazine;
        currentAmmoStock = ammoStock;        
    }

    public virtual void Charge(float chargeTime)
    {
        weaponHolder.CancelReload(); 
        // Lógica general de disparo (si aplica a todas las armas)
    }

    public virtual void Shoot()
    {     
        weaponHolder.CancelReload(); 
        if (currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reload());
            return; // Termina la función para evitar que el disparo se ejecute
        }   
        // Lógica general de disparo (si aplica a todas las armas)
    }

    public virtual IEnumerator Reload()
    {        
        
        StopCoroutine("Reload");        

        isReloading = true;
        float elapsedTime = 0f;
        float reloadDuration = reloadTime;

        while (elapsedTime < reloadDuration)
        {
            float progress = elapsedTime / reloadDuration;
            elapsedTime += Time.deltaTime;
            hudManager.rechargeBar(progress);

            yield return null;
        }

        isReloading = false;
        hudManager.rechargeBar(1f);

        int reloadAmmo = ammoMagazine - currentAmmo;

        if (currentAmmoStock >= ammoMagazine)
        {
            currentAmmo += reloadAmmo;
            currentAmmoStock -= reloadAmmo;
        }
        else
        {
            currentAmmo = currentAmmoStock;
            currentAmmoStock -= currentAmmo;  
            
            
        }

        //explosionParticles.Stop();
    }   


    

}
