using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggerNog : MonoBehaviour
{
    public static JuggerNog Instance;    
    public Character_Script character;    


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivaJugger(){       
        character.speed = 80;
        Debug.Log("Tienes jugger");
        //restapuntos
    }


}
