using UnityEngine;
using Player;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _jumpDuration;
    
    private Animator _animator;
    private JumpPadArea _jumpPadArea;
    private Transform _target;

    void Start()
    {
        _jumpPadArea = transform.parent.GetComponent<JumpPadArea>();
        _animator = _jumpPadArea.GetComponent<Animator>();
        _target = _jumpPadArea.Target;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(Consts.Tags.PLAYER)) return;

        HandleJumpActions();
        
        ActivateJumpPad(other.GetComponent<PlayerController>());
    }

    private void ActivateJumpPad(PlayerController playerController) =>
        playerController.Jump(_target, _jumpPower, _jumpDuration);

    private void HandleJumpActions()
    { 
        SoundManager.Instance.PlaySFX(SoundManager.Instance.JumpPadSfx);
        
        _animator.enabled = true;

        Invoke(nameof(CloseAnimator), 1f);

        _target.gameObject.SetActive(false);
    }

    private void CloseAnimator() =>
        _animator.enabled = false;
}