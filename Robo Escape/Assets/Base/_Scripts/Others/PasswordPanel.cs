using TMPro;
using UnityEngine;

public class PasswordPanel : MonoBehaviour
{
    [SerializeField] private string _password;
    [SerializeField] private TMP_Text _passwordText;
    [SerializeField] private PasswordGate _passwordGate;

    private string _passwordInput ="";

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
            // Alarm Aktifleştirilicek.
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
