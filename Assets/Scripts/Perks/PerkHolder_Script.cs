using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkHolder_Script : MonoBehaviour
{
    public Character_Script character;
    public static Perk_Script perk;
    public static PerkHolder_Script Instance;

    public JuggerNog juggerNog;

    public GameObject buyingPerk;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipPerk(GameObject buyingPerk){        

        if(buyingPerk.name == "JuggerNog"){   
            character.speed = 80;
            Debug.Log("Tienes jugger");          
            JuggerNog.Instance.ActivaJugger();
        }else{
            Debug.Log(":d");  
        }                    
       
         
    }
}
