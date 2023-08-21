using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public static Character_Script character;  
    public int doorCost;
    public string doorName;

    public bool isBought = false;
    private bool onRange = false;


    public GameObject body;
    public GameObject leftDoor;
    public GameObject rightDoor;

    public Sprite openDoorSpriteL;
    public Sprite openDoorSpriteR ;

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
                OpenDoor();  
            }                
        } 
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {          
            if(isBought){
                Debug.Log("Door [2500]"); 
            }else{
                Debug.Log("Abril Puerta[2000]"); 
                onRange = true;   
            }
                   
              
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onRange = false;      
    }

    public void OpenDoor(){            
        isBought = true;        
        //transform.child.GetComponent<BoxCollider2D>().enabled = false;   
        body.GetComponent<BoxCollider2D>().enabled = false;
        //GetComponentInChildren<BoxCollider2D>().enabled = false;
        //transform.Find("Body").GetComponent<BoxCollider2D>().enabled = false;   
        leftDoor.GetComponent<SpriteRenderer>().sprite = openDoorSpriteL;
        rightDoor.GetComponent<SpriteRenderer>().sprite = openDoorSpriteR;
        rightDoor.GetComponent<SpriteRenderer>().flipX = false;
        
    }
}

//how to disable a child boxcollider in unity?
