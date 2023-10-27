using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MeleeWeapon
{
    public Character_Script player;
    public Camera cam;  

    private bool isDashing = false;

    public float attackDelay = 3.5f;
    private float lastAttackTime = 0f;
    
    public int damage = 200;

    public CircleCollider2D swordCollider;

    public float chargeDistance = 2f; // La distancia que recorre el dash
    public float chargeDuration = 0.4f;
    

    [Header("-Slash Particles")]    
    public ParticleSystem slashParticles; 
    public ParticleSystem focusParticles;   

    void Start()
    {
        lastAttackTime = -attackDelay; // Inicializa lastAttackTime con un valor que asegure el primer ataque
    }     

    public override void PerformAttack()
    {       

        float currentTime = Time.time;
        if (currentTime - lastAttackTime < attackDelay)
        {
            // Aún no ha pasado suficiente tiempo desde el último ataque, no hacer nada
            return;
        }

        // Se puede realizar un ataque
        lastAttackTime = currentTime;        

        StartCoroutine(DelayedAttack());
    }
    

    IEnumerator DelayedAttack()
    {        
        focusParticles.Play();  
        yield return new WaitForSeconds(1f); // Espera medio segundo
       
        // Resto del código del ataque
        focusParticles.Stop();  
        slashParticles.Stop();
        slashParticles.Play();    

       
        StartCoroutine(FrontCharge());
        swordCollider.enabled = true;

        yield return new WaitForSeconds(chargeDuration); // Espera el tiempo que dura el ataque

        swordCollider.enabled = false;
    }

    public IEnumerator FrontCharge()
    {
        
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dashDirection = (mousePos - transform.position).normalized;

        if (!isDashing)
        {
            isDashing = true;
            player.myCollider.enabled = false;
            

            Vector3 originalPosition = transform.position;
            Vector3 dashEndPosition = originalPosition + (dashDirection * chargeDistance * 5f);

            float startTime = Time.time;
            while (Time.time < startTime + chargeDuration)
            {
                float t = (Time.time - startTime) / (chargeDuration + 0.1f);
                player.GetComponentInParent<Rigidbody2D>().MovePosition(Vector3.Lerp(originalPosition, dashEndPosition, t));

                
                yield return null;
            }

            isDashing = false;
            player.myCollider.enabled = true;
            
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, swordCollider.radius);

        foreach (Collider2D hit in hitEnemies)
        {
            // Asegúrate de que el objeto encontrado tenga el tag "Enemy"
            if (hit.CompareTag("Enemy"))
            {
                // Reducimos la salud del enemigo
                Enemy_Script enemy = hit.GetComponent<Enemy_Script>();
                enemy.ReduceHealth(damage);
            }
        }
    }


    
}
