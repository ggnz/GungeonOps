using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{ 
    private HUDManager hudManager;
    private Score score;
    public Camera_Script camShake;

    private bool onRange = false;
    private bool puedeReparar = true;
    public bool isFull;
    public bool isEmpty;

    public GameObject body;
    public GameObject[] Boards;   

    private List<GameObject> elementosDesactivados = new List<GameObject>(); 
        
    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>(); 
        score = FindObjectOfType<Score>();   
        camShake = FindObjectOfType<Camera_Script>(); 
    }
   
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interaction")) && onRange && !isFull)
        {
            Reparar();
        }

        // Verificar condiciones para mostrar u ocultar la información de barricadas
        if (isEmpty){body.GetComponent<Collider2D>().enabled = false;}
        else{body.GetComponent<Collider2D>().enabled = true;}  

        //Evita el el jugador salga
        if (onRange){body.GetComponent<Collider2D>().enabled = true;}
        else {body.GetComponent<Collider2D>().enabled = false;}            
   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {       
        if(collision.gameObject.CompareTag("Player") && !isFull) {      
            hudManager.ShowBarricadesInfo(); 
            onRange = true;                                                                             
        } 
        else if (collision.gameObject.CompareTag("Player") && isFull)
        {
            hudManager.HideBarricadesInfo();
            onRange = true;   
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {        
        if(collision.gameObject.CompareTag("Player")) {    
            onRange = false;             
            hudManager.HideBarricadesInfo(); 
        } 
        /*if(collision.gameObject.CompareTag("Enemy")) {    
            Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();           
            enemyRigidbody.bodyType = RigidbodyType2D.Static;            
        }        */           
    }    

    public void Reparar()
    {
        if (!puedeReparar)
        {
            return; // No permite reparar si no puede
        }
        camShake.StartShake(0.1f, 0.05f);        

        isEmpty = false;                     
        GameObject ultimoElementoDesactivado = elementosDesactivados[elementosDesactivados.Count - 1];
        ultimoElementoDesactivado.SetActive(true);         
        score.SumaPuntos(25);                   
        elementosDesactivados.RemoveAt(elementosDesactivados.Count - 1);  

        if (elementosDesactivados.Count <= 0) {           
            isFull = true;           
            hudManager.HideBarricadesInfo();                 
        }

        StartCoroutine(EsperarParaReparar()); // Inicia la corrutina de espera
    }

    private IEnumerator EsperarParaReparar()
    {
        puedeReparar = false; // Deshabilita la reparación
        yield return new WaitForSeconds(1.0f); // Espera 1 segundo (ajusta este valor según lo necesites)
        puedeReparar = true; // Habilita la reparación nuevamente
    }

    public void Destruir(){       

        if (Boards.Length > 0 && elementosDesactivados.Count <= 3) {
            GameObject ultimoElemento = Boards[Boards.Length - 1]; 

            ultimoElemento.SetActive(false); 

            elementosDesactivados.Add(ultimoElemento); 

            // Mover el siguiente elemento al final del array
            GameObject siguienteElemento = Boards[0]; // Obtener el siguiente elemento
            List<GameObject> nuevaLista = new List<GameObject>(Boards); // Convertir el array en una lista
            nuevaLista.RemoveAt(0); // Eliminar el primer elemento
            nuevaLista.Add(siguienteElemento); // Añadir el siguiente elemento al final
            Boards = nuevaLista.ToArray(); // Convertir la lista de nuevo a un array


            if(elementosDesactivados.Count > 0){                                 
                isFull = false;   
            } 
            if(elementosDesactivados.Count >= 4){            
                isEmpty = true;   
            }
            
        }
        
    }

    public void RepararAll(){
        //Carpenter        
        
        while (elementosDesactivados.Count > 0) {
            GameObject ultimoElementoDesactivado = elementosDesactivados[elementosDesactivados.Count - 1];
            ultimoElementoDesactivado.SetActive(true);   
            elementosDesactivados.RemoveAt(elementosDesactivados.Count - 1); 
        }
        isFull = true;   
        isEmpty = false; 
    }

  
}
