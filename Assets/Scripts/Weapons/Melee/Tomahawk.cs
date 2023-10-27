using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomahawk : MeleeWeapon
{
    public int damage = 50;
    public float throwForce = 8f;
    public float returnForce = 15f;
    public Transform player;
    public Transform holder;
    public Rigidbody2D rb;
    public Camera cam;  

    public bool isReturning = false;
    public bool isThrowing = false;
    public float maxReturnDistance = 8f;
    public float currentDistance;

    public float attackCooldown = 2f; // Tiempo en segundos entre ataques
    private bool isOnCooldown = false; // Bandera para controlar el cooldown

    [Header("-Throw Particles")] 
    public ParticleSystem throwParticles;  
    public ParticleSystem hitParticles;   


    public void Start(){
        
    }

    public override void PerformAttack()
    {
        if (!isOnCooldown && !isThrowing)
        {
            ThrowTomahawk();
        }
    }

    void ThrowTomahawk()
    {
        isThrowing = true;
        throwParticles.Stop();
        throwParticles.Play();  

        rb.bodyType = RigidbodyType2D.Dynamic;
        

        Vector3 direccionLanzamiento = transform.right; // Cambia esto según la dirección deseada
        rb.AddForce(direccionLanzamiento * throwForce, ForceMode2D.Impulse);  
        
        if (transform.parent.eulerAngles.y == 0)
        {
            rb.angularVelocity = -1000f;
        }
        else
        {
            rb.angularVelocity = 1000f;
        }  
        

        transform.parent = null;
       


    }

    void Update()
    {
        if (isReturning)   //Vuelta
        {
            Return();
              
        }else{             //Ida         
            currentDistance = Vector3.Distance(transform.position, player.position);

            if (currentDistance > maxReturnDistance)
            {
                isReturning = true;
            } 

        }

        
    }

    void OnTriggerEnter2D(Collider2D other) // Asumiendo que estás usando 2D
    {
        if (!isReturning && other.CompareTag("Wall"))
        {
            isReturning = true;
        }
        else if (isReturning && other.CompareTag("Player"))
        {
            Grab();
        }
        else if (other.CompareTag("Enemy")){
            hitParticles.Play();
            
            Enemy_Script enemy = other.GetComponent<Enemy_Script>();
            if (enemy != null && isThrowing)
            {
                enemy.ReduceHealth(damage);
                maxReturnDistance += 1f;
            }
        }

        
    }

    void Return()
    {
        Vector3 direction = player.position - transform.position;
        rb.velocity = direction.normalized * returnForce;      

        if (direction.magnitude < 0.5f) // Si está lo suficientemente cerca del jugador
        { 
            Grab();
        }  
    }

    

    public void Grab(){
        isThrowing = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;        
        rb.bodyType = RigidbodyType2D.Kinematic;
        isReturning = false; // Dejar de regresar            
        transform.parent = holder; // Volver a ser hijo de su padre original        
        
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPos = transform.parent.position;
        Vector3 direction = (mousePos - playerPos).normalized;  
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Vector3 orbitPosition = playerPos + (direction); 
        transform.position = orbitPosition; 

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

        throwParticles.Stop();
        StartCoroutine(StartCooldown());

    }

    IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
       
    }

    
    
}
