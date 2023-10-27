using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI incomingScore;
    public TextMeshProUGUI scoreText;

    public ParticleSystem explosionParticles;
    public ParticleSystem scoreParticles;

    public bool areDouble = false;

    [Header("-Colors")]
    public Color scoreTextColor;   
    public Color scoreTextParticles; 
    public Color incomingColorNoDouble;   

    public Color colorParaNoDouble1; 
    public Color colorParaNoDouble2; 

    public Color colorParaResta; 
    

    [Header("-DoublePoints")]
    public Color scoreTextColorDouble; 
    public Color scoreTextParticlesDouble;  
    public Color incomingColorDouble; 

    public Color colorParaDouble1; 
    public Color colorParaDouble2;   
    

    void Start()
    {
        scoreText.text = Mathf.Round(score).ToString();
        incomingScore.text = "";
        ParticleSystem.MainModule main = scoreParticles.main;
    }
    
    void Update()
    {     
        if(areDouble){            
            scoreText.color = scoreTextColorDouble;  
            ParticleSystem.MainModule main = scoreParticles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(scoreTextParticlesDouble, scoreTextParticlesDouble);          
        }else{
            scoreText.color = scoreTextColor;
            ParticleSystem.MainModule main = scoreParticles.main;
            main.startColor = new ParticleSystem.MinMaxGradient(scoreTextParticles, scoreTextParticles);          
        }   
    }

    public void RestaPuntos(int puntos)
    {
        score -= puntos;
        scoreText.text = Mathf.Round(score).ToString();
        StartCoroutine(ShowIncomingScore(puntos, false));
    }

    public void SumaPuntos(int puntos)
    {        
        if(areDouble){
            puntos *= 2;            
        }
        score += puntos;
        scoreText.text = Mathf.Round(score).ToString();
        StartCoroutine(ShowIncomingScore(puntos, true));
    }

    private IEnumerator ShowIncomingScore(int puntos, bool isAdding)
    {
        TextMeshProUGUI newIncomingScore = Instantiate(incomingScore, incomingScore.transform.parent);
        newIncomingScore.text = (isAdding ? "+" : "-") + Mathf.Round(puntos).ToString();

        float startTime = Time.time;
        float duration = Random.Range(1f, 2f);

        explosionParticles.Play();
        ParticleSystem.MainModule main = explosionParticles.main; 

        if (areDouble)
        {
            main.startColor = new ParticleSystem.MinMaxGradient(colorParaDouble1, colorParaDouble2);
            newIncomingScore.color = incomingColorDouble;
        }
        else
        {            
            main.startColor = new ParticleSystem.MinMaxGradient(colorParaNoDouble1, colorParaNoDouble2);
            newIncomingScore.color = incomingColorNoDouble;        
            
        }

        Vector3 initialPosition = newIncomingScore.transform.position + Vector3.up * Random.Range(-5f, 5f);
        Vector3 targetPosition = newIncomingScore.transform.position + Vector3.up * Random.Range(-20f, 20f) + Vector3.right * 10f; 
        Color initialColor = newIncomingScore.color;
        
        if(!isAdding){
            initialColor = colorParaResta;
        }

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            newIncomingScore.transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
            newIncomingScore.color = Color.Lerp(initialColor, Color.clear, progress);

            yield return null;
        }
       
        explosionParticles.Stop();
        Destroy(newIncomingScore.gameObject);
    }
}
