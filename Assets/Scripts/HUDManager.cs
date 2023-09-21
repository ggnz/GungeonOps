using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI  healthText;
    
    public TextMeshProUGUI  ammoText;
    public TextMeshProUGUI  magazineText;    
    public TextMeshProUGUI  weaponName;

    public TextMeshProUGUI  roundText;
    public TextMeshProUGUI  buyText;
    public TextMeshProUGUI  infoText;
    public TextMeshProUGUI  windowText;

    public GameObject perkHolder;
    public List<Sprite> perkLogos = new List<Sprite>();

    public GameObject powerUpHolder;
    public List<Sprite> powerUpLogos = new List<Sprite>();
 

    void Start(){
        HideBuyInfo();
        infoText.text = null;
    }

    //Stats
    public void UpdateAmmo(int currentAmmo ,int ammoStock, string nombreArma) {
        ammoText.text = currentAmmo.ToString();
        magazineText.text = ammoStock.ToString();
        weaponName.text = nombreArma;
    }

    public void UpdateHealth(float currentHealth){
        healthText.text = currentHealth.ToString();
    }


    //Info (Doors, Box, Perks)
    public void ShowBuyInfo(string buyInfoAction, string buyInfoName, int buyInfoCost){        
        buyText.text = "Hold F to " + buyInfoAction + " " + buyInfoName + " Cost [" + buyInfoCost + "]";
    }
    public void HideBuyInfo() {
        buyText.text = "";
    }

    public void ShowInfo(string message) {
        infoText.text = message;
        StartCoroutine(DisplayForDuration(infoText));
    }
    IEnumerator DisplayForDuration(TextMeshProUGUI HUDtext) ///Funciona para los powerUp
    {
        float displayDuration = 1.0f;
        float fadeDuration = 0.5f;
        
        HUDtext.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            HUDtext.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HUDtext.alpha = 0f;
    }

    

    //WallBuy & Box
    public void ShowWeaponInfo(string nombreArma) {
        buyText.text = "Hold F to " + nombreArma;
    }

    public void ShowWallWeaponInfo(string nombreArma, int precioArma) {
        buyText.text = "Hold F to " + nombreArma + " Cost [" + precioArma + "]";
    }

    //Barricades
    public void ShowBarricadesInfo() {
        windowText.text = "Hold F to repair Barricade";        
    }

    public void HideBarricadesInfo() {
        windowText.text = "";
        
    }

    //Perks
    public void AddPerkLogo(Sprite logo)
    {
        GameObject newPerkImage = new GameObject("PerkImage");
        Image imageComponent = newPerkImage.AddComponent<Image>();
        imageComponent.sprite = logo;

        // Asigna el PerkHolder como padre
        newPerkImage.transform.SetParent(perkHolder.transform, false);

        // Ajusta la posición de la nueva imagen en la fila
        RectTransform rt = newPerkImage.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(perkLogos.Count * 100, 0); // Ajusta el valor 50 según el espacio que desees entre las imágenes

        // Agrega el logo a la lista de PerkLogos
        perkLogos.Add(logo);
    
    }

    //PowerUps
   public void AddPowerUpLogo(Sprite logo)
    {
        // Verifica si el logo ya existe en la lista de powerUpLogos
        int existingLogoIndex = powerUpLogos.IndexOf(logo);

        if (existingLogoIndex != -1)
        {
            // Si el logo ya existe, actualiza su imagen
            Image existingImage = powerUpHolder.transform.GetChild(existingLogoIndex).GetComponent<Image>();
            existingImage.sprite = logo;
        }
        else
        {
            // Si no existe, crea un nuevo objeto de imagen y añádelo a powerUpHolder
            GameObject newPowerUpImage = new GameObject("PerkImage");
            Image imageComponent = newPowerUpImage.AddComponent<Image>();
            imageComponent.sprite = logo;

            newPowerUpImage.transform.SetParent(powerUpHolder.transform, false);

            RectTransform rt = newPowerUpImage.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(powerUpLogos.Count * 100, 0);

            powerUpLogos.Add(logo);
        }
    }

    public void HidePowerUpLogo(Sprite logo)
    {
        int index = powerUpLogos.IndexOf(logo);

        // Si se encuentra el logo
        if (index != -1)
        {
            // Elimina el logo de la lista
            powerUpLogos.RemoveAt(index);

            // Elimina el GameObject del logo del powerUpHolder
            Destroy(powerUpHolder.transform.GetChild(index).gameObject);

            // Reajusta las posiciones de las imágenes restantes
            for (int i = index; i < powerUpHolder.transform.childCount; i++)
            {
                RectTransform rt = powerUpHolder.transform.GetChild(i).GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(i * 100, 0);
            }
        }
        
    }

    //Rounds
    public void ShowRoundInfo(int round) {
        roundText.text = round.ToString();              
    }
   

}