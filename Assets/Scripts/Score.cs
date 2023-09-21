using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    public int score;
    public TextMeshProUGUI incomingScore;
    //public TextMeshProUGUI outcomingScore;
    public TextMeshProUGUI scoreText;

    public Color incoming;
    public Color outcoming;

    public bool areDouble = false;


    void Start()
    {
        //score = 500;
        scoreText.text = Mathf.Round(score).ToString();
        incomingScore.text = "";
    }
    
    void Update()
    {        
        if(areDouble){            
            scoreText.color = new Color(0.4f, 0.9f, 0.4f, 1.0f);
        }else{
            scoreText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        
    }

    public void RestaPuntos(int puntos)
    {
        score -= puntos;
        scoreText.text = Mathf.Round(score).ToString();
        StartCoroutine(ShowIncomingScore(puntos, false, outcoming));
        //outcomingScore.text = Mathf.Round(puntos).ToString();
    }

    public void SumaPuntos(int puntos)
    {        
        if(areDouble){
            puntos *= 2;            
        }
        score += puntos;
        scoreText.text = Mathf.Round(score).ToString();
        StartCoroutine(ShowIncomingScore(puntos, true, incoming));
        //incomingScore.text = Mathf.Round(puntos).ToString();
    }


    private IEnumerator ShowIncomingScore(int puntos, bool isAdding, Color textColor)
    {
        // Crear una instancia nueva del objeto Text
        TextMeshProUGUI newIncomingScore = Instantiate(incomingScore, incomingScore.transform.parent);
        newIncomingScore.text = (isAdding ? "+" : "-") + Mathf.Round(puntos).ToString();

        float startTime = Time.time;
        float duration = Random.Range(1f, 2f); // Duración en segundos de la animación  

        Vector3 initialPosition = newIncomingScore.transform.position + Vector3.up * Random.Range(-5f, 5f);
        Vector3 targetPosition = newIncomingScore.transform.position + Vector3.up * Random.Range(-70f, 70f) + Vector3.right * 70f; // Mueve el texto hacia arriba
        Color initialColor = isAdding ? 
            textColor + Random.ColorHSV(0.1f, 0.15f, 0.8f, 1.0f, 0.8f, 1.0f)
            : 
            textColor
        ;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            newIncomingScore.transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
            newIncomingScore.color = Color.Lerp(initialColor, Color.clear, progress); // Hace que el texto se desvanezca gradualmente

            yield return null;
        }
       
        Destroy(newIncomingScore.gameObject);
    }

}
