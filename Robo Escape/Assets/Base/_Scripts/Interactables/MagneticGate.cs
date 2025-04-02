using UnityEngine;

public class MagneticGate : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();    
        _animator.enabled = false;
    }

    public void OpenGate()
    {
        transform.parent = null;
        _animator.enabled = true;
    }
}
