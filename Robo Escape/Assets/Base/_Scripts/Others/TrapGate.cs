using UnityEngine;

public class TrapGate : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen = true;

    void Start()
    {
        _animator = GetComponent<Animator>();    
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Consts.Tags.PLAYER) && _isOpen)
        _animator.SetTrigger(Consts.AnimationParameters.CLOSE);
    }
}
