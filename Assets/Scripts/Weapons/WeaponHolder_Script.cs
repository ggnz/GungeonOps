using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder_Script : MonoBehaviour
{
    public Character_Script character;
    public HUDManager hudManager;
    public MeleeHolder meleeHolder;

    [Header("-Armas")]
    public int selectedWeapon = 0;
    public Weapon_Script primaryWeapon;
    public Weapon_Script secondaryWeapon;

    [Header("-Granadas")]
    public GameObject granade;
    private GameObject nuevaGranada;
    public float maxImpulseTime = 2f; // Tiempo máximo de impulso en segundos
    public float impulseForceMultiplier = 10f; // Multiplicador de fuerza

    public int currentGranadeCount = 3; // Inicializamos con 3 granadas
    public int maxGranadeCount = 3;
    
    
    private bool isThrowing = false;
    private float throwStartTime;
    private bool canThrow = true; // Variable para controlar si se puede lanzar una granada
    private float throwCooldown = 1.5f;

    [Header("-AIM")]
    public Camera cam;   
    Vector2 movement;
    Vector2 mousepos;

    
    public enum Modes
    {
        mk,
        controller,        
        
    }
    public Modes InputMode;

    
    void Start()
    {        
        
        SelectWeapon();
        character = FindObjectOfType<Character_Script>();  
        meleeHolder = FindObjectOfType<MeleeHolder>();  
        hudManager = FindObjectOfType<HUDManager>();  
        primaryWeapon = transform.GetChild(0).GetComponent<Weapon_Script>();
        secondaryWeapon = transform.GetChild(1).GetComponent<Weapon_Script>();       
    }

    void Update()
    {     

        //Granadas//
        if ((Input.GetKeyDown(KeyCode.Mouse3) || Input.GetButtonDown("Granade")) && canThrow && currentGranadeCount > 0) // Cambia esto por el botón que desees
        {
            isThrowing = true;
            throwStartTime = Time.time; 
            EquipGranade();                    
        }

        if (isThrowing) {
            // Mantiene la granada siguiendo la posicion del WeaponHolder
            nuevaGranada.transform.position = transform.position;
        }

        if ((Input.GetKeyUp(KeyCode.Mouse3) || Input.GetButtonUp("Granade")) && isThrowing)
        {
            
            isThrowing = false;
            float holdTime = Time.time - throwStartTime;            
            
            // Calcula la fuerza de impulso en función del tiempo sostenido
            float impulseForce = Mathf.Clamp(holdTime * impulseForceMultiplier, 0f, maxImpulseTime * impulseForceMultiplier);

            ThrowGrande(impulseForce);
        }
        
        
        //Cambio de Arma//
        int previousSelectedWeapon = selectedWeapon;
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        //Mouse
        if (scrollWheel != 0f) {
            meleeHolder.unequipMelee();
            selectedWeapon = (selectedWeapon + (scrollWheel > 0f ? -1 : 1) + transform.childCount) % transform.childCount;
            CancelReload();                                      
        }          
        //Control
        if (Input.GetButtonUp("ChangeWeapon")){                  
            selectedWeapon = (selectedWeapon + 1) % transform.childCount;            
            CancelReload();   
        } 

        if (previousSelectedWeapon != selectedWeapon )
        {
            SelectWeapon();
            primaryWeapon = transform.GetChild(selectedWeapon).GetComponent<Weapon_Script>();
            secondaryWeapon = transform.GetChild(previousSelectedWeapon).GetComponent<Weapon_Script>();                        
            
            
        }  


        //Aim 
        Vector3 aimDirection;

        if(InputMode == Modes.mk){
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10; 
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            aimDirection = (targetPosition - transform.parent.position).normalized;
        }
        else{
            //Aim con control       
            float aimHorizontal = Input.GetAxis("AimHorizontal");
            float aimVertical = Input.GetAxis("AimVertical");
            aimDirection = new Vector3(aimHorizontal, aimVertical, 0).normalized;
        }

        // Calcular la nueva posición de orbita
        Vector3 orbitPosition = transform.parent.position + aimDirection * 0.5f;
        transform.position = orbitPosition; 

        // Calcular el ángulo y establecer la rotación
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        bool estaApuntandoArriba = angle > 30f && angle < 150f;
        bool estaApuntandoAbajo = angle > -150f && angle < -30f;
        bool estaApuntandoDerecha = (angle >= -30f && angle <= 30f) || (angle <= -150f && angle >= 150f);
        bool estaApuntandoIzquierda = angle > 150f || angle < -150f;

        character.animator.SetBool("isAimingUp", estaApuntandoArriba);
        character.animator.SetBool("isAimingDown", estaApuntandoAbajo);
        character.animator.SetBool("isAimingRight", estaApuntandoDerecha);
        character.animator.SetBool("isAimingLeft", estaApuntandoIzquierda);

        if (angle > -90f && angle < 90f)
        {
            // No invertir eje Y
            transform.rotation *= Quaternion.Euler(new Vector3(0f, 0, 0));
        }
        else
        {
            // Invertir eje Y
            transform.rotation *= Quaternion.Euler(new Vector3(180f, 0, 0));
        }  
      
          

    }

    public void EquipWeapon(Weapon_Script buyingWeapon)
    {
        // Verificar si ya se tiene un arma equipada
        if (primaryWeapon != null)
        {
            
            // Si hay un arma equipada, reemplazarla
            Transform selectedWeaponTransform = transform.GetChild(selectedWeapon);
            Destroy(selectedWeaponTransform.gameObject);
            
        }

        Weapon_Script nuevaArma = Instantiate(buyingWeapon, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f)) as Weapon_Script;
        nuevaArma.transform.parent = transform;
        nuevaArma.transform.SetSiblingIndex(selectedWeapon);
        nuevaArma.gameObject.SetActive(true);
        nuevaArma.name = buyingWeapon.name;
        primaryWeapon = nuevaArma; // Ahora primaryWeapon sera la nueva arma comprada
        
    }

    void SelectWeapon ()
    {       
        
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform weapon = transform.GetChild(i);
            weapon.gameObject.SetActive(i == selectedWeapon);                
            
        }
        
    }

    public void RecargarMaximo() //Max Ammo (PowerUp)
    {
        primaryWeapon.FillAmmo();
        secondaryWeapon.FillAmmo();
        FillGranades();        
    }

    public bool HasWeapon(Weapon_Script weapon) {
        return primaryWeapon == weapon || secondaryWeapon == weapon;
    }

    public void RechargeMaxAmmo(Weapon_Script weapon) {  //Max Ammo (PARED)
        weapon.FillAmmo();    

        
    }

    public void CancelReload(){
        primaryWeapon.isReloading = false;
        StopCoroutine("Reload"); 
        hudManager.rechargeBar(1f); 
    }




    ////////////////////////////////    Granadas     //////////////////////////////////////

    public void EquipGranade(){
        // Crea una instancia de la granada en la posición de lanzamiento
        nuevaGranada = Instantiate(granade, transform.position, Quaternion.identity);

        nuevaGranada.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Ajusta los valores según lo necesites

        // Hacer que la granada sea hijo del jugador
        nuevaGranada.transform.parent = transform;

        Collider2D collider = nuevaGranada.GetComponent<Collider2D>();            
        collider.enabled = false;
    }

    public void ThrowGrande(float impulseForce){ 
        
        Granade granada = nuevaGranada.GetComponent<Granade>();            
        granada.trailParticles.Play();

        currentGranadeCount--;  
        nuevaGranada.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // Ajusta los valores según lo necesites
        hudManager.UpdateGranadeCount(currentGranadeCount);
        // Deshacer el padre para que la granada deje de seguir al jugador
        nuevaGranada.transform.parent = null;
        

        

        Collider2D collider = nuevaGranada.GetComponent<Collider2D>();            
        collider.enabled = true;

        Vector3 direccionLanzamiento = transform.right; // Cambia esto según la dirección deseada
        
        // Obtén el Rigidbody de la granada (si tiene uno) y aplica una fuerza para simular el lanzamiento
        Rigidbody2D rb = nuevaGranada.GetComponent<Rigidbody2D>();        
        rb.AddForce(direccionLanzamiento * impulseForce, ForceMode2D.Impulse);   

        StartCoroutine(StartThrowCooldown());              
        
    }

    IEnumerator StartThrowCooldown()
    {
        canThrow = false; // Desactivamos la posibilidad de lanzar
        yield return new WaitForSeconds(throwCooldown); // Esperamos el tiempo de cooldown
        canThrow = true; // Activamos la posibilidad de lanzar nuevamente
    }


    public void FillGranades(){
        currentGranadeCount = maxGranadeCount;
        hudManager.UpdateGranadeCount(currentGranadeCount);
    }


    public void ChangeGranade(GameObject newGranade){
        granade = newGranade;
        
        // Enviar el Sprite como parámetro a la función ChangeGranadeIcon
        hudManager.ChangeGranadeIcon(newGranade.GetComponent<SpriteRenderer>().sprite);
    }

    
}
