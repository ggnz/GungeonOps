using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{ 
    public HUDManager hudManager;
    public Score score;

    private bool onRange = false;
    public bool isFull;
    public bool isEmpty;

    public GameObject body;
    public GameObject[] Boards;   

    public List<GameObject> elementosDesactivados = new List<GameObject>(); 
        
    void Start()
    {
        hudManager = FindObjectOfType<HUDManager>(); 
        score = FindObjectOfType<Score>();    
    }
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && onRange && !isFull)
        {
            Reparar();
        }

        // Verificar condiciones para mostrar u ocultar la información de barricadas
        
              

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onRange = true;
        if(collision.gameObject.CompareTag("Player") && !isFull) {      
            hudManager.ShowBarricadesInfo();                                                                           
        } 

        else if (collision.gameObject.CompareTag("Player") && isFull)
        {
            hudManager.HideBarricadesInfo();
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onRange = false;
        if(collision.gameObject.CompareTag("Player")) {    
            onRange = false;  

            hudManager.HideBarricadesInfo(); 
        }                   
    }

    public void Reparar(){  
        isEmpty = false;                     
        GameObject ultimoElementoDesactivado = elementosDesactivados[elementosDesactivados.Count - 1];
        ultimoElementoDesactivado.SetActive(true);         
        score.SumaPuntos(25);                   
        elementosDesactivados.RemoveAt(elementosDesactivados.Count - 1);  

        if(elementosDesactivados.Count <= 0) {           
            isFull = true;           
            hudManager.HideBarricadesInfo();                 
        }              
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
            Reparar();
        }
        isFull = true;   
        isEmpty = false; 
    }

  
}
