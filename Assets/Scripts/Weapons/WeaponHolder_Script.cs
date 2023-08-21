using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder_Script : MonoBehaviour
{
    public Character_Script character;
    public static Weapon_Script weapon;
    public static WeaponHolder_Script Instance;
    public int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {        
        SelectWeapon();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)        
        {
            if (selectedWeapon >= transform.childCount - 1){
                selectedWeapon = 0;
            }               
            else{
                selectedWeapon++;
            }
                
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon <= 0 ){
                selectedWeapon = transform.childCount - 1;
            }                
            else{
                selectedWeapon--;
            }
            
        }
            
        if (previousSelectedWeapon != selectedWeapon )
        {
            SelectWeapon();
        } 
        
   
    }

    public void EquipWeapon(GameObject buyingWeapon){                 //, string nombre        
        int i = 0;
        foreach (Transform weapon in transform)
        {    
            if (i == selectedWeapon){   //si el arma esta equipada entonces...             
                //destruye el arma   
                Destroy(weapon.gameObject);          

                //crea y posiciona el nuevo arma   
                GameObject nuevaArma = Instantiate(buyingWeapon, character.transform.position, transform.rotation * Quaternion.Euler (0f, 0f, 0f)) as GameObject; 
                nuevaArma.transform.parent = this.transform;               
                nuevaArma.transform.SetSiblingIndex(selectedWeapon); 
                
                
                //activa el nuevo arma
                nuevaArma.gameObject.SetActive(true);   
                
                //cambia el nombre para que no aparezca el "(clone)"
                nuevaArma.name = buyingWeapon.name;  
                                                                       
            }
            i++; //revisa el siguiente arma           
        }


           
    }

    void SelectWeapon ()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {    
            if (i == selectedWeapon){
                weapon.gameObject.SetActive(true); //si el arma es la equipada la Activa
                
            }
             
            else{
                weapon.gameObject.SetActive(false);//si el arma NO es la equipada la Desactiva
            }
            i++;
                    
        }
    }

  
    
}


//how to drop a child for its parent unity?