using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Script : MonoBehaviour
{
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
        //private bool cdRegen = false;

    
    void Start()
    {
        
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
    }

}











