using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float tiempoDeExplosion;
    public float radioDeExplosion;
    public int damage;   
    public Camera_Script cam;

    public float shakeA = 0.05f;
    public float shakeD = 0.05f;    

    public ParticleSystem explosionParticles; 
    public ParticleSystem trailParticles;  

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public virtual IEnumerator DestruirGranada()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeExplosion);
    }  
}
