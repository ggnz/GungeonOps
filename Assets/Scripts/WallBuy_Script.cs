using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuy_Script : MonoBehaviour
{
    //private Score score;
    public static Character_Script character;
    public static WeaponHolder_Script weaponHolder;
    public GameObject wallWeapon;
    
    //private BuyInfo buyInfo;

    public int weaponCost;
    public string weaponName;

    private bool isBought = false;
    private bool onRange = false;

    public static WallBuy_Script wallBuy;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && onRange) {                                           
            if(isBought){
                //BuyAmmo(wallWeapon);  
            }else{                
                BuyWeapon(wallWeapon);  
            }                
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {          
            if(isBought){
                Debug.Log("Ammo [1000]"); 
            }else{
                Debug.Log("Comprar Arma de pared [2000]"); 
                onRange = true;   
            }
                   
              
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onRange = false;      
    }

    public void BuyWeapon(GameObject wallWeapon){         
        WeaponHolder_Script.Instance.EquipWeapon(wallWeapon);
        isBought = true;   
    }
}

//how to equip a weapon in unity?




//Source: https://stackoverflow.com/questions/73008973











