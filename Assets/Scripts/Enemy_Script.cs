using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Script : MonoBehaviour
{  
    public Score score;
    public Transform player;
    public NavMeshAgent agente;
    public Barricade barricade;
    public PowerUpHandler powerUpHandler;
    public Spawn spawn;

    public bool isDestroying = false;
    
    //private int currentWaypointIndex = 0;

    public float moveSpeed = 3.5f; // Velocidad de movimiento de los enemigos.
    public float attackRange = 1.5f; // Rango de ataque.
    public int attackDamage = 10; // Daño del ataque.
    
    private float timeSinceLastAttack = 0f;
    private bool isAttacking = false;
    public float attackCooldown = 2f;

    public int currentHealth;
    public int maxHealth;
    public int puntosPorKill = 100;
    //public float increasePerWave;

    public Color lowHealthColor = Color.red; // Color para cuando la vida es 1

    private Renderer enemyRenderer;
    private Color originalColor;

    public void Start(){ 
        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;          
    
        spawn = FindObjectOfType<Spawn>(); 
        powerUpHandler = FindObjectOfType<PowerUpHandler>(); 
        score = FindObjectOfType<Score>(); 
        player = GameObject.FindGameObjectWithTag("Player").transform;           
        
        agente = GetComponent<NavMeshAgent>();
        agente.updateRotation = false;
        agente.updateUpAxis = false;
    }

    public void Awake(){
        currentHealth = maxHealth;        
    }

    private void Update()
    {       
        //Efecto visual para Instakill
        if (powerUpHandler.isInsta)
        {
            enemyRenderer.material.color = lowHealthColor;
        }
        else
        {
            enemyRenderer.material.color = originalColor;
        }

        agente.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);        

        if (distanceToPlayer <= attackRange)
        {
            // El jugador está dentro del rango de ataque.
            if (!isAttacking)
            {
                // Verifica si ha pasado suficiente tiempo desde el último ataque.
                if (Time.time - timeSinceLastAttack >= attackCooldown)
                {
                    AttackPlayer();
                    timeSinceLastAttack = Time.time; // Actualiza el tiempo del último ataque.
                }
            }
        }  

        if (barricade.isEmpty){
            agente.speed = moveSpeed;
        }             
    }  

    private void AttackPlayer()
    {        
        isAttacking = true;        
        if (player != null)
        {
            player.GetComponent<Character_Script>().takeDamage();
        }

        StartCoroutine(ResetAttackState());        
    }
    private IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }  
    
////////////////////////////////////////////////////////////////////////////////////////
    

    public void ReduceHealth(int damage)
    {
        if(powerUpHandler.isInsta){ //Efecto para Instakill
            IsDead();            
        }else{
            currentHealth -= damage;
            score.SumaPuntos(10);
            if(currentHealth <= 0 ){
                IsDead();
            }
        }
        
        
    }
    public void IsDead()
    {        
        score.SumaPuntos(puntosPorKill); 
        DropPowerUp();            
        Destroy(this.gameObject); 
        spawn.DisminuirEnemigosVivos();          
          
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Barricade") && !barricade.isEmpty)
        {                               
            StartCoroutine(DestroyBarricade());                  
        }        
    }

    private IEnumerator DestroyBarricade()
    {
        isDestroying = true; // El personaje está dentro del trigger
        while (isDestroying)
        {
            agente.speed = 0f;
            barricade.Destruir();
            yield return new WaitForSeconds(2f); // Espera durante 2 segundos antes de la próxima llamada
            
        }

        
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void DropPowerUp()
    {        
        powerUpHandler.SpawnRandomPowerUp(transform.position);
        
    }

    

}
