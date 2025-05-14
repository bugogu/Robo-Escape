using UnityEngine;

public class PasswordGate : MonoBehaviour
{
    [SerializeField] private PasswordPanel _passwordPanel;
    private Animator _animator;

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
