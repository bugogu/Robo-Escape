using UnityEngine;

public class TrapGate : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen = true;
    private int _closeTriggerHash = Animator.StringToHash(Consts.AnimationParameters.CLOSE);

    void Start() =>
        _animator = GetComponent<Animator>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tags.PLAYER) && _isOpen)
            TriggerAction();
    }

    private void TriggerAction()
    { 
        _isOpen = false;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.GateClosingSfx);
        _animator.SetTrigger(_closeTriggerHash);
    }
}
