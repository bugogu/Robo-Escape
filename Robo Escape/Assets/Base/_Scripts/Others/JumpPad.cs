using UnityEngine;

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
        _target = _jumpPadArea.target;
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(Consts.Tags.PLAYER)) return;

        SoundManager.Instance.PlaySFX(SoundManager.Instance.jumpPadSfx);
        
        _animator.enabled = true;

        Invoke(nameof(CloseAnimator), 1f);
        
        ActivateJumpPad(other.GetComponent<PlayerController>());
        
        _target.gameObject.SetActive(false);
    }

    private void ActivateJumpPad(PlayerController playerController)
    {
        playerController.Jump(_target, _jumpPower, _jumpDuration);
    }

    private void CloseAnimator()=>
        _animator.enabled = false;
}
