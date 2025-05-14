using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TMPro.TMP_Text FpsText; 
    public float UpdateInterval = 0.5f; 
    
    private float _accum = 0f; 
    private int _frames = 0; 
    private float _timeLeft; 
    private float _fps = 0f; 

    void Start()
    {
        _timeLeft = UpdateInterval;
    }

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        _frames++;
        
        if (_timeLeft <= 0f)
        {
            _fps = _accum / _frames;
            FpsText.text = Mathf.Round(_fps).ToString();
            
            _timeLeft = UpdateInterval;
            _accum = 0f;
            _frames = 0;
        }
    }
}
