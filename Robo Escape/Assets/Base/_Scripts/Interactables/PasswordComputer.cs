using TMPro;
using UnityEngine;

public class PasswordComputer : MonoBehaviour
{
    [SerializeField] private Transform _newParent;
    [SerializeField] private string _password;

    void Start()
    {
        _newParent.parent = null;

        transform.SetParent(_newParent);
    }

    public void ShowPassword() =>
        GetComponent<TextMeshPro>().text = _password;
}