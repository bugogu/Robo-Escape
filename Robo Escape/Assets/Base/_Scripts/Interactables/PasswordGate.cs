using UnityEngine;

public class PasswordGate : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private PasswordPanel _passwordPanel;

    void Start()
    {
        // _passwordPanel = FindAnyObjectByType<PasswordPanel>(); // Sahne başlangıcında obje pasif olduğu için ulaşılamaz.

        _animator = GetComponent<Animator>();    
        _animator.enabled = false;
    }

    public void OpenPanel()=>
        _passwordPanel.gameObject.SetActive(true);

    public void OpenGate()
    {
        transform.parent = null;
        _animator.enabled = true;
    }
}
