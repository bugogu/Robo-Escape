using UnityEngine;

[RequireComponent ( typeof ( Rigidbody ) )]
public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _runSpeed = 0;
    [SerializeField] private float _turnSpeed = 0;

    private Rigidbody _rbPlayer;
    private float _horizontal = 0;
    private float _vertical = 0;

    #endregion

    private void Awake() => Initial();

    private void Initial() => _rbPlayer = GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
            JoystickMovement();
        else
        {
            _animator.SetBool(Consts.PlayerAnimations.WALKING, false);
            _rbPlayer.linearVelocity = Vector3.zero;
        }
    }

    public void JoystickMovement()
    {
        _animator.SetBool(Consts.PlayerAnimations.WALKING,true);
        _horizontal = dynamicJoystick.Horizontal;
        _vertical = dynamicJoystick.Vertical;

        Vector3 newPos = new Vector3(_horizontal * _runSpeed * Time.fixedDeltaTime, 0, _vertical * _runSpeed * Time.fixedDeltaTime);
        _rbPlayer.linearVelocity = newPos;

        Vector3 dir = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _turnSpeed * Time.fixedDeltaTime);
    }

}
