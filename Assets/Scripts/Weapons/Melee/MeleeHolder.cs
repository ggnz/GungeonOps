using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHolder : MonoBehaviour
{
    public MeleeWeapon currentWeapon; 
    public WeaponHolder_Script weaponHolder;

    [Header("-AIM")]
    public Camera cam;   
    Vector2 movement;
    Vector2 mousepos;

    public bool meleeEquiped;

    void Start()
    {            
        weaponHolder = FindObjectOfType<WeaponHolder_Script>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EquipMelee();             
            
        }
        

        if (Input.GetMouseButtonDown(0) && meleeEquiped)
        {
            // Realizar el ataque si hay un arma melee equipada y se hace clic izquierdo
            PerformMeleeAttack();
        }

        Vector3 aimDirection;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10; 
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        aimDirection = (targetPosition - transform.parent.position).normalized;     

  
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        Vector3 orbitPosition = transform.parent.position + aimDirection * 0.5f;
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
    }

    public void EquipMelee(){

        weaponHolder.primaryWeapon.gameObject.SetActive(false);
        
        // Activa el arma melee (puedes cambiar "Sword" por el nombre del objeto de tu espada)
        currentWeapon.gameObject.SetActive(true);
        meleeEquiped = true;
    }

    public void unequipMelee(){      
        currentWeapon.gameObject.SetActive(false);
        meleeEquiped = false;
        
    }
   
    public void PerformMeleeAttack()
    {
        if (currentWeapon != null)
        {
            // Coloca aquí la lógica para el ataque melee, por ejemplo, activar animaciones, causar daño, etc.
            currentWeapon.PerformAttack();
        }
    }
}
