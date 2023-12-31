using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Doors : MonoBehaviour
{
   

    private static Character_Script character;  
    private HUDManager hudManager;
    private Score score;
    private Spawn spawnScript;
    public Transform[] nuevosSpawns;    

   [Header("-Door Stats")]
    public int doorCost;
    public string doorName;
    public string doorAction;

    public bool isBought = false;
    private bool onRange = false;


    public GameObject body;  

    // Start is called before the first frame update
    void Start()
    {
        //navMeshSurface = FindObjectOfType<NavMeshSurface>();

        spawnScript = FindObjectOfType<Spawn>(); 
        hudManager = FindObjectOfType<HUDManager>(); 
        score = FindObjectOfType<Score>();    
        
              
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interaction")) && onRange) {                                           
            if(isBought){
                //BuyAmmo(wallWeapon);  
            }else{                
                OpenDoor(); 
                //navmesh.BuildNavMeshAsync();
            }                
        } 
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {          
            if(isBought){                
                
            }else{                
                onRange = true;   
                hudManager.ShowBuyInfo(doorAction, doorName, doorCost);
            }                               
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onRange = false;      
        hudManager.HideBuyInfo();    
    }

    public void OpenDoor(){        
            
        spawnScript.HabilitarNuevosSpawns(nuevosSpawns);

        hudManager.HideBuyInfo();  
        score.RestaPuntos(doorCost);    

        isBought = true;        

        body.GetComponent<BoxCollider2D>().enabled = false;

        Destroy(this.gameObject);     
        
    }
}

