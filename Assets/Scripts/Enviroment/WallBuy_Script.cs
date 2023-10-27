using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuy_Script : MonoBehaviour
{    
    private WeaponHolder_Script weaponHolder;
    private HUDManager hudManager;  
    public GameObject wallWeapon;    
    
    [Header("-Wall Weapon")]
    public int weaponCost;
    public string weaponName;    

    public int ammoCost;
    private string ammoName = "buy ammo";   

    private bool onRange = false;

    public Score score;

    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();     
        weaponHolder = FindObjectOfType<WeaponHolder_Script>();   
        score = FindObjectOfType<Score>();         
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interaction")) && onRange) 
        {                                           
            if (weaponHolder.HasWeapon(wallWeapon.GetComponent<Weapon_Script>())) 
            { 
                // Recargar al máximo la munición si el arma ya ha sido comprada
                if (score.score >= ammoCost) {
                    score.RestaPuntos(ammoCost);  
                    weaponHolder.RechargeMaxAmmo(wallWeapon.GetComponent<Weapon_Script>()); 
                } else {
                    hudManager.ShowInfo("No tienes suficiente dinero");      
                }
                
            } 
            else
            {       
                if (score.score >= weaponCost) {
                    hudManager.HideBuyInfo();  
                    score.RestaPuntos(weaponCost);  
                    StartCoroutine(EsperarYBuyWeapon());   
                } else {
                    hudManager.ShowInfo("No tienes suficiente dinero");          
                }
            }                
        } 
    }

    private IEnumerator EsperarYBuyWeapon() {        
        yield return new WaitForSeconds(0.5f);         
        BuyWeapon(wallWeapon);  
    } 

    public void BuyWeapon(GameObject wallWeapon)
    {           
        weaponHolder.EquipWeapon(wallWeapon.GetComponent<Weapon_Script>());
        //weapon = wallWeapon.GetComponent<Weapon_Script>();                    
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player"))
        {          
            if (weaponHolder.HasWeapon(wallWeapon.GetComponent<Weapon_Script>())) 
            {                 
                hudManager.ShowWallWeaponInfo(ammoName, ammoCost);                
            }
            else
            {
                hudManager.ShowWallWeaponInfo(weaponName, weaponCost);                  
            }
            onRange = true;                    
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  {
        onRange = false;    
        hudManager.HideBuyInfo();      
    }
}
