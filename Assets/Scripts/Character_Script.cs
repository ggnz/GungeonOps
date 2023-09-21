using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Script : MonoBehaviour
{
    public HUDManager hudManager;

    [Header("-MOVEMENT")]
        public float speed = 8f;
        public Rigidbody2D characterRB;

    [Header("-AIM")]
        public Camera cam;   
        Vector2 movement;
        Vector2 mousepos;

    [Header("-HEALTH")]
        public float currentHealth = 100f;
        public float maxHealth = 100f;
        public int regenSpeed= 2;
        //private float tiempo = 0f;
        public float delayParaRegenerar = 1.5f;
        public Image hurtScreen;
        //private bool cdRegen = false;
            

        
    
    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>();
        hurtScreen.GetComponent<Image>().color = new Color(255, 0, 0, 0.0f);
    }

    
    void Update()
    {
        //Movimiento      
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        characterRB.MovePosition(characterRB.transform.position + tempVect);


        //Aim        
        mousepos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 mousePos = Input.mousePosition;

        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
        hudManager.UpdateHealth(currentHealth);    
        

        if (angle > -90f && angle < 90f)
        {
            // Aplicar rotación en el eje X (por ejemplo, 180 grados)
            transform.rotation *= Quaternion.Euler(new Vector3(0f, 0, 0));
        }else{
            transform.rotation *= Quaternion.Euler(new Vector3(180f, 0, 0));
        }
        if(Input.GetKey(KeyCode.M)) {   
            takeDamage();             
        } 
    }


    public void takeDamage(){
        currentHealth -= 5.0f;  
        Debug.Log("Ouch");
        HurtScrren();
    }

    public void HurtScrren(){
        if (currentHealth <= maxHealth * 1)
        {
            hurtScreen.GetComponent<Image>().color = new Color(255, 0, 0, 0.0f);
        }
        if (currentHealth <= maxHealth * 0.75)
        {
            hurtScreen.GetComponent<Image>().color = new Color(255, 0, 0, 0.25f);
        }
        if (currentHealth <= maxHealth * 0.5)
        {
            hurtScreen.GetComponent<Image>().color = new Color(255, 0, 0, 0.5f);
        }
        if (currentHealth <= maxHealth * 0.25)
        {
            hurtScreen.GetComponent<Image>().color = new Color(255, 0, 0, 0.75f);
        }
        if (currentHealth <= maxHealth * 0.1)
        {
            hurtScreen.GetComponent<Image>().color = new Color(255, 0, 0, 0.9f);
        }
    }

}











