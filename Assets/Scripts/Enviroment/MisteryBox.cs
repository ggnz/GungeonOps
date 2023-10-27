using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisteryBox : MonoBehaviour
{  
    
    [Header("-Objects")]   
    public WeaponHolder_Script weaponHolder;
    public BoxHolder_Script boxHolder; 

    public Weapon_Script[] guns; //Pool de armas de la caja
    public GameObject[] granades;
    public Weapon_Script oso;
    public Weapon_Script granade;

    public Weapon_Script boxResult; //Arma que sale en la caja (Incluye Oso)    
    public Weapon_Script buyingWeapon; //Arma que sale en la caja (No incluye Oso)

    public HUDManager hudManager;
    public Score score;
    
    //Flags
    public bool isOpen = false; 
    private bool onRange = false;
    public bool osoAvailable = false;

    [Header("-Sprites")]  
    public SpriteRenderer weaponHolderSprite;
    public SpriteRenderer spriteCaja;
    public Color openBoxColor; //color cuando la caja esta abierta    
    public Color closeBoxColor;
    

    [Header("-Variables")]  
    private string nombreCaja = "MisteryBox";
    private string nombreArma;
    private string action = "buy";
    public int cost = 950;
    public float rouletteTime = 2.0f;
    public float tiempoCierraCaja = 10f;
    private int attempts;
    public int attemptsParaOso;

    [Header("-Box Particles")] 
    public ParticleSystem explosionParticles; 
    public ParticleSystem lightParticles;     

    [Header("-Oso Particles")]     
    public ParticleSystem weaponGlow; 
    public ParticleSystem osoParticles;  
    

    void Start() {   
        weaponHolder = FindObjectOfType<WeaponHolder_Script>();      
        boxHolder = GetComponentInParent<BoxHolder_Script>();
        hudManager = FindObjectOfType<HUDManager>();  
        score = FindObjectOfType<Score>();      
        
    }

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interaction")) && onRange) { 
                                                 
            if(isOpen == false){   //Cuando la caja aun no se ha abierto               
                Roulette();                  
                
            }else{          //Cuando ya hay resultado en la caja   
                if(boxResult == granade){
                    GameObject randomGranade = granades[Random.Range(0, granades.Length)]; 
                    weaponHolder.ChangeGranade(randomGranade); 
                    CierraCaja();
                }else{
                    BuyWeapon(buyingWeapon);
                }            
                
            }                
        } 

        //Oso
        if(attempts > attemptsParaOso){
            osoAvailable = true;
        }else{
            osoAvailable = false;
        }        
        
    }   
  

    public void Roulette(){
        
        explosionParticles.Play();
        isOpen = true;
        score.RestaPuntos(950);
        spriteCaja.color = openBoxColor;        

        //Escoje aleatoriamente el arma excluyendo el arma primaria y la secundaria    
        int rouletteNumber;
        int maxIndex = guns.Length - (osoAvailable ? 0 : 1);
        //El Oso se encuentra en la ultima posicion del array
        //Si el oso NO esta disponible, reduce el array para evitar que salga
        //Por el contrario, abarca todo el array, incluyendo el Oso 
        do{
            rouletteNumber = Random.Range(0, maxIndex);
        }while (
            guns[rouletteNumber].name == weaponHolder.primaryWeapon.name 
            ||
            guns[rouletteNumber].name == weaponHolder.secondaryWeapon.name
        );     

        
        StartCoroutine(rouletteResult(rouletteNumber));
        
        
    }

    private IEnumerator rouletteResult(int rouletteNumber)
    {
        yield return new WaitForSeconds(rouletteTime); // 2 segundos de retraso

        
        boxResult = guns[rouletteNumber]; 
      
        weaponHolderSprite.sprite = boxResult.GetComponent<SpriteRenderer>().sprite;

        attempts++;         

        if(boxResult == oso ){ //si sale Oso        
            onRange = false;      
            osoParticles.Play();     
            lightParticles.Stop();     
            weaponGlow.Play();   
            weaponHolderSprite.sprite = null; 
            ParticleSystem.MainModule main = explosionParticles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(Color.black);    
            StartCoroutine(EsperarYSaleOso()); 
            
        }
        else {                       
            buyingWeapon = boxResult; 
            nombreArma = boxResult.gameObject.name; 
            hudManager.ShowWeaponInfo(nombreArma);
            //cost = boxResult.gameObject.costoArma; 
        }    
        
        StopCoroutine("EsperaYCierraCaja");
        StartCoroutine("EsperaYCierraCaja");
    }

    private IEnumerator EsperaYCierraCaja()
    {
        yield return new WaitForSeconds(tiempoCierraCaja); //Variable TiempoCierraCaja
        
        CierraCaja();
        //StopCoroutine(EsperaYCierraCaja());   
    }




    //Oso
    private System.Collections.IEnumerator EsperarYSaleOso() {        
        yield return new WaitForSeconds(3.5f);         
        saleOso();
    }

    public void saleOso(){   
        
        weaponGlow.Stop();   
        score.SumaPuntos(950); 
        attempts = 0;       
        buyingWeapon = oso;         
        CierraCaja();      
        osoParticles.Stop();   
        boxHolder.cambiaLocalizacion(); 
             
    }


    public void BuyWeapon(Weapon_Script buyingWeapon){            
        weaponHolder.EquipWeapon(buyingWeapon);
        CierraCaja();  
    }

    public void CierraCaja(){  
        
        buyingWeapon = null;
        hudManager.HideBuyInfo();   
        isOpen = false; 
        spriteCaja.color = closeBoxColor;        
        weaponHolderSprite.sprite = null;   
        explosionParticles.Stop();
    }
  

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player"))
        {          
            if(isOpen == false){            
                hudManager.ShowBuyInfo(action, nombreCaja, cost);                
            }else{
                hudManager.ShowWeaponInfo(nombreArma);
                
            }   
            onRange = true;                          
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        hudManager.HideBuyInfo();
        onRange = false;      
    }

}
