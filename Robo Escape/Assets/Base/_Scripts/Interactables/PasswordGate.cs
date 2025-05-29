using UnityEngine;

public class PasswordGate : MonoBehaviour
{
    public InteractionPlate InteractionPlatePasswordGate;
    [SerializeField] private PasswordPanel _passwordPanel;
    private Animator _animator;

    void Start()
    {
        // _passwordPanel = FindAnyObjectByType<PasswordPanel>(); // Sahne başlangıcında obje pasif olduğu için ulaşılamaz.

        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        transform.parent = null;
    }

    public void OpenPanel() =>
        _passwordPanel.gameObject.SetActive(true);

    public void OpenGate() =>
        _animator.enabled = true;
}