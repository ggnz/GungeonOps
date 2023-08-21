using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Perk_Script : MonoBehaviour
{
    public static Character_Script character;
    public static JuggerNog JuggerNog_Script;
    public GameObject perk;
    

    public int machineCost;
    public string machineName;

    public bool isBought = false;
    private bool onRange = false;


    void Start()
    {
  
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && onRange) {                                           
            if(isBought){
                //BuyAmmo(wallWeapon);  
            }else{                
                BuyPerk(perk);  
            }                
        } 
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {          
            if(isBought){
                Debug.Log("Jugger [2500]"); 
            }else{
                Debug.Log("Comprar Perk[2000]"); 
                onRange = true;   
            }
                   
              
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onRange = false;      
    }

    public void BuyPerk(GameObject perk){    
        JuggerNog.Instance.ActivaJugger(); 
        isBought = true;        
        //restapuntos
    }
}
