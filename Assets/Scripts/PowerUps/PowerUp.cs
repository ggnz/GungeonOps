using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Score score;
    public enum PowerUps
    {
        DoublePoints,
        Instakill,
        Carpenter,
        Nuke,
        BonusPoints,
        MaxAmmo

    } public PowerUps powerUp;   

    private PowerUpHandler powerUpHandler;

    public Sprite logo;

    private HUDManager hudManager;

    private WeaponHolder_Script weaponHolder;

    private Spawn spawn; 

    private Renderer objectRenderer;
    public float destroyTime = 12f;
    public float fadeDuration = 3f;

    public float powerUpEffectDuration = 30f;

    public Camera_Script cam; 


    public ParticleSystem explosionParticles;  


    public void Start(){     
        objectRenderer = GetComponent<Renderer>();   
        score = FindObjectOfType<Score>(); 
        weaponHolder = FindObjectOfType<WeaponHolder_Script>(); 
        spawn = FindObjectOfType<Spawn>(); 
        powerUpHandler = FindObjectOfType<PowerUpHandler>();
        hudManager = FindObjectOfType<HUDManager>(); 
        cam = FindObjectOfType<Camera_Script>();
    }

    void Awake()
    {
        StartCoroutine(DestroyAfterDelay(destroyTime)); 
        explosionParticles.Play();       
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOut(fadeDuration));        
    }

    private IEnumerator FadeOut(float duration)
    {
        explosionParticles.Stop();       
        float startTime = Time.time;
        Color startColor = objectRenderer.material.color;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            objectRenderer.material.color = newColor;
            yield return null;
        }

        // Asegúrate de establecer la opacidad a 0 al final para evitar cualquier pequeña discrepancia
        Color finalColor = startColor;
        finalColor.a = 0f;
        objectRenderer.material.color = finalColor;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) {      
            Destroy(this.gameObject);
            switch (powerUp)
            {
                case PowerUps.DoublePoints:
                    DoublePoints();                
                    break;
                case PowerUps.Instakill:
                    Instakill();
                    break;    
                case PowerUps.Carpenter:
                    Carpenter();
                    break;  
                case PowerUps.Nuke:
                    Nuke();  
                    break; 
                case PowerUps.BonusPoints:
                    BonusPoints();
                    break;  
                case PowerUps.MaxAmmo:
                    MaxAmmo();
                    break;    
                                                                                                                        
            }      
        
        }
    }

    

    public void DoublePoints()
    {    
        if (powerUpHandler.activeDoublePointsCoroutine != null)
        {
            powerUpHandler.StopCoroutine(powerUpHandler.activeDoublePointsCoroutine);
        }
        
        // Inicia una nueva instancia de la corrutina y guarda la referencia
        powerUpHandler.activeDoublePointsCoroutine = powerUpHandler.StartCoroutine(powerUpHandler.ActiveDoublePoints(powerUpEffectDuration, logo));

        hudManager.AddPowerUpLogo(logo);
    }

    public void Instakill(){
        if (powerUpHandler.activeInstakillCoroutine != null){
            powerUpHandler.StopCoroutine(powerUpHandler.activeInstakillCoroutine);
        }
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");        
        
        foreach (GameObject enemy in enemies) {
            Enemy_Script enemyScript = enemy.GetComponent<Enemy_Script>();            
            powerUpHandler.activeInstakillCoroutine = powerUpHandler.StartCoroutine(powerUpHandler.ActiveInstakill(enemyScript,powerUpEffectDuration, logo));
        }
        hudManager.AddPowerUpLogo(logo);
    }  

    public void Nuke(){ 
        cam.StartShake(1.5f, 0.03f);       
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    
        foreach (GameObject enemy in enemies) {
            Enemy_Script enemyScript = enemy.GetComponent<Enemy_Script>();                     
            enemyScript.StartCoroutine(enemyScript.IsDeadNuke());           
    
        }

        score.SumaPuntos(500); // Aumenta 500 cada 5 rondas
    }

    public void Carpenter(){
        GameObject[] barricades = GameObject.FindGameObjectsWithTag("Barricade");
    
        foreach (GameObject barricade in barricades) {
            Barricade barricadeScript = barricade.GetComponent<Barricade>();
            barricadeScript.RepararAll();
        }

        score.SumaPuntos(500); // Aumenta 500 cada 5 rondas
    }

    public void BonusPoints(){        
        int valorAleatorio = Random.Range(1, 101); 
        int points; 

        if (valorAleatorio <= 1) // 1% de probabilidad
        {
            points = 25000;
        }
        else if (valorAleatorio <= 10) // 9% de probabilidad
        {
            points = 5000;
        }
        else if (valorAleatorio <= 20) // 10% de probabilidad
        {
            points = 2500;
        }
        else if (valorAleatorio <= 50) // 30% de probabilidad
        {
            points = 1000; // Cambiado a 1000
        }
        else // 50% de probabilidad
        {
            points = 500;
        }

        score.SumaPuntos(points); 

    }

    public void MaxAmmo(){
        weaponHolder.RecargarMaximo();
    }

    public void FireSale(){
        
    }

    public void ZombieBlood(){
        
    }

}
