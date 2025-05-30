using UnityEngine;

public class PasswordPanel : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _passwordText;
    [SerializeField] private GameObject _passwordBlankText;
    [SerializeField] private PasswordGate _passwordGate;

    private string _password;

    private string _passwordInput = "";

    void Start()
    {
        _password = LevelManager.Instance.LevelData.Password;
    }

    void Update()
    {
        _passwordText.text = _passwordInput;

        if (!(_passwordInput.Length >= _password.Length)) return;

        if (_passwordInput == _password)
            HandleTruePassword();
        else
            HandleFalsePassword();
    }

    void OnEnable()
    {
        GameManager.Instance.ChangeGameState(GameState.Pause);
    }

    public void AddDigit(string digit)
    {
        _passwordBlankText.SetActive(false);
        _passwordInput += digit;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.DigitSfx);
    }

    public void CancelPanel()
    {
        FindAnyObjectByType<PasswordGate>().InteractionPlatePasswordGate.ReActive();
        ClosePasswordPanel();
        _passwordBlankText.SetActive(true);
        Settings.Instance.PlayButtonSound();
    }

    private void ClosePasswordPanel()
    {
        ClearPasswordInput();
        gameObject.SetActive(false);
        GameManager.Instance.ChangeGameState(GameState.Play);
    }

    private void HandleTruePassword()
    {
        HandleAnimationAndGameState();
        ClosePasswordPanel();
    }

    private void HandleFalsePassword()
    {
        if (Settings.Instance.Haptic == 1) Handheld.Vibrate();

        SensorArea[] allSensors = FindObjectsByType<SensorArea>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var sensor in allSensors)
            sensor.SetTriggeredColor();

        GameManager.Instance.SetAlarm(true, true);
        HandleAnimationAndGameState();
        ClosePasswordPanel();
    }

    private void HandleAnimationAndGameState()
    { 
        _passwordGate.OpenGate();
        GameManager.Instance.ChangeGameState(GameState.Play);
    }

    private void ClearPasswordInput() => _passwordInput = ""; 
}