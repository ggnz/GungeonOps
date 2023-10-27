using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy_Script : MonoBehaviour
{  
    private bool ignorePlayer = false;


    public Score score;
    public Transform player;
    public NavMeshAgent agente;
    public Barricade barricade = null;
    public PowerUpHandler powerUpHandler;
    public Spawn spawn;

    public bool isDestroying = false;
    
    //private int currentWaypointIndex = 0;

    public float moveSpeed = 3.5f; // Velocidad de movimiento de los enemigos.
    public float attackRange = 1.5f; // Rango de ataque.
    public int attackDamage = 10; // Daño del ataque.
    
    private float timeSinceLastAttack = 0f;
    protected bool isAttacking = false;
    public float attackCooldown = 2f;

    public int currentHealth;
    public int maxHealth;
    public int puntosPorKill = 100;
    //public float increasePerWave;

    public Color lowHealthColor = Color.red; // Color para cuando la vida es 1

    protected Renderer enemyRenderer;
    protected Color originalColor;


    public Transform targetToChase;


    public ParticleSystem explosionParticles;   
    public ParticleSystem deadParticles; 


 

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
        agente.speed = moveSpeed;


        targetToChase = player;
    }

    public void Awake(){
        currentHealth = maxHealth;        
    }

    protected virtual void Update()
    {     

        agente.SetDestination(targetToChase.position);
          
        //Efecto visual para Instakill
        if (powerUpHandler.isInsta)
        {
            enemyRenderer.material.color = lowHealthColor;
        }
        else
        {
            enemyRenderer.material.color = originalColor;
        }

        if (targetToChase != null)
        {
            Vector3 direction = targetToChase.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
        }
        

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

    protected abstract void AttackPlayer(); 

    protected  IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    } 
    
////////////////////////////////////////////////////////////////////////////////////////
    

    public void ReduceHealth(int damage)
    {
        explosionParticles.Play();
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
    public virtual void IsDead()
    {         
        score.SumaPuntos(puntosPorKill); 
        DropPowerUp();            
        Destroy(this.gameObject); 
        spawn.DisminuirEnemigosVivos();          
          
    }

    public IEnumerator IsDeadNuke()
    {    
        deadParticles.Play();

        yield return new WaitForSeconds(1f); // Espera durante 2 segundos

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

    public void StopChasingPlayerTemporarily(float stopTime)
    {
        if (!ignorePlayer)
        {
            ignorePlayer = true; // Ignora al jugador temporalmente.

            // Desactiva el agente de navegación.
            agente.isStopped = true;

            // Invoca una función para reanudar el seguimiento después de 'stopTime' segundos.
            Invoke("ResumeChasingPlayer", stopTime);
        }
    }

    private void ResumeChasingPlayer()
    {
        ignorePlayer = false; // Vuelve a seguir al jugador.
        agente.isStopped = false; // Activa el agente de navegación.
    }


    //Mono

    public void ChangeTargetTemporarily(Transform newTarget, float changeTime)
    {
        if (newTarget != null)
        {
            
            // Cambia el objetivo que sigue el agente de navegación.
            targetToChase = newTarget;

            // Desactiva el agente de navegación temporalmente.
            //agente.isStopped = true;

            // Invoca una función para restaurar el objetivo y reanudar el seguimiento después de 'changeTime' segundos.
            Invoke("ResumeChasingOriginalTarget", changeTime);
        }
    }

    private void ResumeChasingOriginalTarget()
    {
        // Reactiva el agente de navegación.
        //agente.isStopped = false;     
        // Vuelve a seguir al jugador.
        targetToChase = player;
        
    }
    
    

}
