using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisteryBox : MonoBehaviour
{  
    
    [Header("-Objects")]   
    public WeaponHolder_Script weaponHolder;
    public BoxHolder_Script boxHolder; 

    public Weapon_Script[] guns; //Pool de armas de la caja
    public Weapon_Script oso;

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
    

    //Otras Variables
    private string nombreCaja = "MisteryBox";
    private string nombreArma;
    private string action = "buy";
    private int cost = 950;
    private float rouletteTime = 2.0f;
    private int attempts;
    

    void Start() {   
        weaponHolder = FindObjectOfType<WeaponHolder_Script>();      
        boxHolder = GetComponentInParent<BoxHolder_Script>();
        hudManager = FindObjectOfType<HUDManager>();  
        score = FindObjectOfType<Score>();      
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && onRange) { 
                                                 
            if(isOpen == false){   //Cuando la caja aun no se ha abierto               
                Roulette();                  
                
            }else{          //Cuando ya hay resultado en la caja             
                BuyWeapon(buyingWeapon);
            }                
        } 

        if(attempts > 5){
            osoAvailable = true;
        }else{
            osoAvailable = false;
        }        
        
    }   
  

    public void Roulette(){ 
       
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
            guns[rouletteNumber].gameObject.name == weaponHolder.primaryWeapon.name 
            ||
            guns[rouletteNumber].gameObject.name == weaponHolder.secondaryWeapon.name
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
            StartCoroutine(EsperarYSaleOso()); 
            
        }else{                       
            buyingWeapon = boxResult; 
            nombreArma = boxResult.gameObject.name; 
            hudManager.ShowWeaponInfo(nombreArma);
            //cost = boxResult.gameObject.costoArma; 
        }       
    }



    //Oso
    private System.Collections.IEnumerator EsperarYSaleOso() {        
        yield return new WaitForSeconds(3.5f);         
        saleOso();
    }

    public void saleOso(){   
        score.SumaPuntos(950); 
        attempts = 0;       
        buyingWeapon = oso;         
        CierraCaja();      
        boxHolder.cambiaLocalizacion();      
    }


    public void BuyWeapon(Weapon_Script buyingWeapon){            
        weaponHolder.EquipWeapon(buyingWeapon);
        CierraCaja();  
    }

    public void CierraCaja(){     
        hudManager.HideBuyInfo();   
        isOpen = false; 
        spriteCaja.color = closeBoxColor;        
        weaponHolderSprite.sprite = null;   
    }
  

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Player"))
        {          
            if(isOpen == false){            
                hudManager.ShowBuyInfo(action, nombreCaja, cost);
                onRange = true;   
            }                          
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        hudManager.HideBuyInfo();
        onRange = false;      
    }

}
