using UnityEngine;

public class TrapGate : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen = true;
    private int _closeTriggerHash = Animator.StringToHash(Consts.AnimationParameters.CLOSE);

    void Start()
    {
        _animator = GetComponent<Animator>();    
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Consts.Tags.PLAYER) && _isOpen)
        {
            _isOpen = false;
            SoundManager.Instance.PlaySFX(SoundManager.Instance.gateClosingSfx);
            _animator.SetTrigger(_closeTriggerHash);
        }
        
    }
}
