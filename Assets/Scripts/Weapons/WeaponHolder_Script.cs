using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder_Script : MonoBehaviour
{
    public Character_Script character;

    public int selectedWeapon = 0;
    public Weapon_Script primaryWeapon;
    public Weapon_Script secondaryWeapon;

    void Start()
    {         
        SelectWeapon();

        primaryWeapon = transform.GetChild(0).GetComponent<Weapon_Script>();
        secondaryWeapon = transform.GetChild(1).GetComponent<Weapon_Script>();       
    }

    void Update()
    {     
        int previousSelectedWeapon = selectedWeapon;

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0f)
        {
            selectedWeapon = (selectedWeapon + (scrollWheel > 0f ? -1 : 1) + transform.childCount) % transform.childCount;
        }

        if (previousSelectedWeapon != selectedWeapon )
        {
            SelectWeapon();
            primaryWeapon = transform.GetChild(selectedWeapon).GetComponent<Weapon_Script>();
            secondaryWeapon = transform.GetChild(previousSelectedWeapon).GetComponent<Weapon_Script>();          
        }         
    }

    public void EquipWeapon(Weapon_Script buyingWeapon)
    {
        // Verificar si ya se tiene un arma equipada
        if (primaryWeapon != null)
        {
            // Si hay un arma equipada, reemplazarla
            Transform selectedWeaponTransform = transform.GetChild(selectedWeapon);
            Destroy(selectedWeaponTransform.gameObject);
        }

        Weapon_Script nuevaArma = Instantiate(buyingWeapon, transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f)) as Weapon_Script;
        nuevaArma.transform.parent = transform;
        nuevaArma.transform.SetSiblingIndex(selectedWeapon);
        nuevaArma.gameObject.SetActive(true);
        nuevaArma.name = buyingWeapon.name;
        primaryWeapon = nuevaArma; // Ahora primaryWeapon será la nueva arma comprada
    }

    void SelectWeapon ()
    {       
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform weapon = transform.GetChild(i);
            weapon.gameObject.SetActive(i == selectedWeapon);
        }
    }

    public void RecargarMaximo()
    {
        primaryWeapon.FillAmmo();
        secondaryWeapon.FillAmmo();
    }

    public bool HasWeapon(Weapon_Script weapon) {
        return primaryWeapon == weapon || secondaryWeapon == weapon;
    }

    public void RechargeMaxAmmo(Weapon_Script weapon) {
        weapon.FillAmmo();    
        
    }
}
