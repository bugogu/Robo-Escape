using TMPro;
using UnityEngine;

public class PasswordPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _passwordText;
    [SerializeField] private PasswordGate _passwordGate;

    private string _password;

    private string _passwordInput ="";

    void Start()
    {
        _password = LevelManager.Instance.levelData.password;
    }

    void Update()
    {
        _passwordText.text = _passwordInput;

        if(!(_passwordInput.Length >= _password.Length)) return;

        if(_passwordInput == _password)
        {
            _passwordGate.OpenGate();
            _passwordInput = "";
            GameManager.Instance.ChangeGameState(GameState.Play);
            gameObject.SetActive(false);
        }
        else
        {
            if(Settings.Instance.Haptic == 1) Handheld.Vibrate();
            
            SensorArea[] allSensors = FindObjectsByType<SensorArea>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var sensor in allSensors)
                sensor.SetTriggeredColor();
                
            GameManager.Instance.SetAlarm(true, true);
            _passwordGate.OpenGate();
            _passwordInput = "";
            GameManager.Instance.ChangeGameState(GameState.Play);
            gameObject.SetActive(false);
        }
    }

    public void AddDigit(string digit)=>
        _passwordInput += digit;

    void OnEnable()
    {
        GameManager.Instance.ChangeGameState(GameState.Pause);
    }
}
