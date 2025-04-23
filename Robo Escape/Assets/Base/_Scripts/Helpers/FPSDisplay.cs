using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TMP_Text fpsText; 
    public float updateInterval = 0.5f; 
    
    private float accum = 0f; 
    private int frames = 0; 
    private float timeLeft; 
    private float fps = 0f; 

    void Start()
    {
        timeLeft = updateInterval;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;
        
        if (timeLeft <= 0f)
        {
            fps = accum / frames;
            fpsText.text = Mathf.Round(fps).ToString();
            
            timeLeft = updateInterval;
            accum = 0f;
            frames = 0;
        }
    }
}
