using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PerkMachine : MonoBehaviour
{     
    private Score score;   
    private HUDManager hudManager;  
    private Character_Script player;   
    private WeaponHolder_Script weaponHolder;   
    

    public int machineCost;
    //public string machineName;

    public enum Perks
    {
        JuggerNog,
        SpeedCola,
        DoubleTap,
        StaminUp      
    }
    public Perks perkInMachine;

    public Sprite logo;
    
    //public Image doubleTapLogo;
    //public Image staminUpLogo;

    private bool isBought = false;
    private bool onRange = false;

    private Dictionary<Perks, bool> perksEstado = new Dictionary<Perks, bool>();

    private string action = "buy";


    void Start()
    {        

        weaponHolder = FindObjectOfType<WeaponHolder_Script>();     
        player = FindObjectOfType<Character_Script>();     
          

       
        hudManager = FindObjectOfType<HUDManager>();  
        score = FindObjectOfType<Score>();      

        foreach(Perks perk in Enum.GetValues(typeof(Perks)))
        {
            perksEstado[perk] = false;
        }
        //weaponScript = weaponHolder.primaryWeapon.GetComponent<Weapon_Script>();
    }
    
    void Update()
    {      

        if((Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interaction")) && onRange) {                                                
            BuyPerk(perkInMachine);            
        }         
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {         
        if(collision.gameObject.CompareTag("Player"))
        {     
            if(!isBought ){  
                hudManager.ShowBuyInfo(action, perkInMachine.ToString(), machineCost);                       
                onRange = true;   
            }                           
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onRange = false;  
        hudManager.HideBuyInfo();    
    }

    public void BuyPerk(Perks perkInMachine){ 
                   
        if (!perksEstado[perkInMachine]) { // Verificar si no ha sido comprado
            if (score.score >= machineCost) {
                score.RestaPuntos(machineCost); 
                perksEstado[perkInMachine] = true; // Marcar como comprado

                // Agregar el logo al HUD
                hudManager.AddPerkLogo(logo);
                hudManager.HideBuyInfo(); 

                switch (perkInMachine)
                {
                    case Perks.JuggerNog:
                        JuggerNog();                
                        break;
                    case Perks.SpeedCola:
                        SpeedCola();
                        break;    
                    case Perks.DoubleTap:
                        DoubleTap();
                        break;  
                    case Perks.StaminUp:
                        StaminUp();
                        break;           
                }
            } else {
                hudManager.ShowInfo("No tienes suficiente dinero");                
            }
        } else {
            hudManager.ShowInfo("Ya has comprado este Perk"); 
        }           
    }

    public void JuggerNog(){       
        player.maxHealth  = 250f;
        player.currentHealth = 250f;
        player.regenSpeed= 3;
    }

    public void SpeedCola(){          
        foreach (Transform weapon in weaponHolder.transform)
        {
            Weapon_Script weaponScript = weapon.GetComponent<Weapon_Script>();
            if (weaponScript != null)
            {
                weaponScript.reloadTime *= 0.9f; // Reducir el tiempo de recarga en un 20%
            }
        }

    }

    public void DoubleTap(){        
        foreach (Transform weapon in weaponHolder.transform)
        {
            Weapon_Script weaponScript = weapon.GetComponent<Weapon_Script>();
            if (weaponScript != null)
            {
                weaponScript.fireRate *= 0.5f; // Aumenta el fireRate en un 50%
            }
        }

    }

    public void StaminUp(){        
        player.speed  = 10f; 
  
    }
}
