using TMPro;
using UnityEngine;

public class PasswordComputer : MonoBehaviour
{
    [SerializeField] private Transform _newParent;
    [SerializeField] private string _password;

    void Start()
    {
        if(_password == "" || string.IsNullOrEmpty(_password))
            _password = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().LevelData.Password;

        if (_newParent != null)
        {
            _newParent.parent = null;
            transform.SetParent(_newParent);
        }
        
    }

    public void ShowPassword() =>
        GetComponent<TextMeshPro>().text = _password;
}