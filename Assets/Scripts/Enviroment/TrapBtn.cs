using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBtn : MonoBehaviour
{
    public Trap trampaScript;    
    public Doors door; 
    public GameObject trampaBody;  
    private HUDManager hudManager;
    private Score score;

    [Header("-Info")]
    public int trapCost;
    public string trapName;
    public string trapAction;

    [Header("-Stats")]
    public float activationDuration = 30f; // Duración de la activación en segundos
    public float cooldownDuration = 60f; // Duración del enfriamiento en segundos
    public int damage = 500;

    [Header("-Flags")]
    public bool inRange = false;
    public bool canActve = false;
    public bool onCoolDown = false;

    public ParticleSystem cdParticle;   
    public ParticleSystem cdParticle2;   
    public Color coolDownColor; 
    public Color readyColor;    
    
    void Start()
    {           
        hudManager = FindObjectOfType<HUDManager>(); 
        score = FindObjectOfType<Score>();   
                    
    }

    void Update()
    {
        if(door.isBought){
            canActve = true;                  
        }        

        if ((Input.GetKeyDown(KeyCode.F) || 
            Input.GetButtonDown("Interaction")) && inRange && canActve && !onCoolDown)
        {
            PressButton();            
        }
    }

    public void PressButton()
    {               
        
        StartCoroutine(trampaScript.ActivateTrap(activationDuration));   
        StartCoroutine(CooldownTrap()); 
        hudManager.HideBuyInfo();                   
        score.RestaPuntos(trapCost);    
    }

    private IEnumerator CooldownTrap()
    {           
        var mainModule = cdParticle.main;
        var mainModuleChild = cdParticle2.main;   

        mainModule.startColor = new ParticleSystem.MinMaxGradient(coolDownColor);        
        mainModuleChild.startColor = new ParticleSystem.MinMaxGradient(coolDownColor);

        onCoolDown = true;  

        yield return new WaitForSeconds(cooldownDuration);        
        onCoolDown = false;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(readyColor);
        mainModuleChild.startColor = new ParticleSystem.MinMaxGradient(readyColor);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && canActve && !onCoolDown)
        {                                
            inRange = true;   
            hudManager.ShowBuyInfo(trapAction, trapName, trapCost);                                       
        }else{

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;      
        hudManager.HideBuyInfo();    
    }


    
}
