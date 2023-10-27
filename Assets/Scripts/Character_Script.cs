using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Character_Script : MonoBehaviour
{
    public HUDManager hudManager;
    public Camera cam;
    public Camera_Script camShake;
    public GameObject weaponHolder;

    [Header("-MOVEMENT")]
        public Animator animator;   
        public float speed = 8f;
        public Rigidbody2D characterRB;

    [Header("-DASH")]
        public float dashDistance = 1.5f; // La distancia que recorre el dash
        public float dashDuration = 0.5f; // La duración del dash en segundos
        public bool isDashing = false; // Variable para controlar si se está realizando un dash
        public float dashCooldown = 2.0f;
        private bool canDash = true;

        public ParticleSystem dashParticles;         

        public ParticleSystem scythDashParticles;   

        public Collider2D myCollider;
        

    [Header("-HEALTH")]
        public float currentHealth = 100f;
        public float maxHealth = 100f;
        public float regenAmount = 10f;
        public int regenSpeed= 5;
        //private float tiempo = 0f;
        public float delayParaRegenerar = 5f;
        public Image hurtScreen;
        public RawImage gameOverScreen;
        public TextMeshProUGUI gameOverText;
        public TextMeshProUGUI pressKeyText;
        //private bool cdRegen = false;
            
        
    
    void Start()
    {
        weaponHolder = transform.Find("WeaponHolder")?.gameObject;
        camShake = FindObjectOfType<Camera_Script>();
        animator = FindObjectOfType<Animator>();
        hudManager = FindObjectOfType<HUDManager>();
        hurtScreen.GetComponent<Image>().color = new Color(255, 0, 0, 0.0f);
        myCollider = GetComponent<Collider2D>();

    }

    
    void Update()
    {

        // Movimiento      
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");          

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        characterRB.MovePosition(characterRB.transform.position + tempVect);

        // Determinar la dirección
        bool isMovingUp = (v > 0);
        bool isMovingDown = (v < 0);
        bool isMovingRight = (h > 0);
        bool isMovingLeft = (h < 0);

        // Actualizar el parámetro "isMoving" en el Animator
        bool isMoving = (h != 0 || v != 0);
        animator.SetBool("isMoving", isMoving);

        // Actualizar los parámetros de dirección
        animator.SetBool("isMovingUp", isMovingUp);
        animator.SetBool("isMovingDown", isMovingDown);
        animator.SetBool("isMovingRight", isMovingRight);
        animator.SetBool("isMovingLeft", isMovingLeft);


        hudManager.UpdateHealth(currentHealth);  

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
        {
            // Comienza el dash
            StartCoroutine(NormalDash());
        }

        animator.SetBool("isDashing", isDashing);
            
    }
    public IEnumerator NormalDash()
    {
        // Obtener la dirección del movimiento
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dashDirection = new Vector3(h, v, 0).normalized;

        if (!isDashing && canDash)
        {
            weaponHolder.SetActive(false);

            //Cancel Reload
            weaponHolder.GetComponent<WeaponHolder_Script>().CancelReload();    
           

            //myCollider.enabled = false;
            
            isDashing = true;
            canDash = false;  
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
          
            
            dashParticles.Stop();
            dashParticles.Play(); 

            Vector3 originalPosition = transform.position;
            Vector3 dashEndPosition = originalPosition + (dashDirection * dashDistance);

            float startTime = Time.time;
            while (Time.time < startTime + dashDuration)
            {
                float t = (Time.time - startTime) / dashDuration;
                characterRB.MovePosition(Vector3.Lerp(originalPosition, dashEndPosition, t));
                yield return null;
            }

            isDashing = false;
            weaponHolder.SetActive(true);
            //myCollider.enabled = true;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
            
            
            

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }


    public IEnumerator Dash()
    {
        
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dashDirection = (mousePos - transform.position).normalized;

        if (!isDashing)
        {
            isDashing = true;
            scythDashParticles.Stop();
            scythDashParticles.Play(); 

            Vector3 originalPosition = transform.position;
            Vector3 dashEndPosition = originalPosition + (dashDirection * dashDistance);

            float startTime = Time.time;
            while (Time.time < startTime + dashDuration)
            {
                float t = (Time.time - startTime) / dashDuration;
                characterRB.MovePosition(Vector3.Lerp(originalPosition, dashEndPosition, t));
                yield return null;
            }

            isDashing = false;
        }
    }    

   

    ////////////////////////////Modificar dano
    public void takeDamage(int damage){
        if(!isDashing){
            currentHealth -= damage;  
            camShake.StartShake(0.5f, 0.2f);            
            HurtScreen();
            if (currentHealth <= 0)
            {
                StartCoroutine(GameOver());
            }

            StopCoroutine("RegenerateHealth");
            StartCoroutine("RegenerateHealth");
        }        
       
    }

    public void HurtScreen()
    {      
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

    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayParaRegenerar);

            while (currentHealth < maxHealth)
            {
                currentHealth += regenAmount;
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                hudManager.UpdateHealth(currentHealth);
                HurtScreen();
                yield return new WaitForSeconds(regenSpeed);
            }
        }
    }

    IEnumerator GameOver()
    {
        Vector3 escalaInicial = cam.transform.localScale;
        Vector3 escalaFinal = new Vector3(0.2f, 0.2f, 1.0f);
        float tiempoDeTransicion = 15.0f;
        float tiempoDeKey = 3.0f;
        float tiempoPasado = 0f;
        camShake.StopShake();

        //hudManager.gameObject.SetActive(false);
        Collider2D miCollider = GetComponent<Collider2D>();
        miCollider.enabled = false;
        speed = 0;        
        weaponHolder.SetActive(false);
        gameOverScreen.GetComponent<RawImage>(); // Obtén la imagen de la pantalla de herida


        while (tiempoPasado < tiempoDeTransicion)
        {
            tiempoPasado += Time.deltaTime;
            cam.transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, tiempoPasado / tiempoDeTransicion);
            
            float t = (tiempoPasado / tiempoDeTransicion) * 5;
            float nuevaTransparencia = Mathf.Lerp(0f, 1f, t); // La transparencia irá de 0 (totalmente transparente) a 1 (totalmente opaca)
            
            float transparenciaEn255 = nuevaTransparencia * 255f; // Convierte de rango 0-1 a 0-255
            
            gameOverScreen.color = new Color(0.078f, 0.070f, 0.121f, transparenciaEn255 / 255f);   
            gameOverText.color = new Color(1f, 0.439f, 0.541f, transparenciaEn255 / 255f);   
            gameOverText.rectTransform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.5f, 1.5f, 1f), t);
        
            if (tiempoPasado > tiempoDeKey){
                pressKeyText.color = new Color(255, 255, 255, 255); 
            }
            yield return null;
            
        }
         

        

        // Asegurémonos de que la cámara tenga el tamaño final exacto
        cam.transform.localScale = escalaFinal;

        // Mantenemos la pantalla de Game Over hasta que el jugador presione una tecla
        yield return new WaitUntil(() => Input.anyKeyDown);

        // Reiniciamos la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }


    

}
