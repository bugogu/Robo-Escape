using UnityEngine;

[RequireComponent ( typeof ( Rigidbody ) )]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool _isMoving;

    [Header("References")]
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _runSpeed = 0;
    [SerializeField] private float _walkSpeed = 0;
    [SerializeField] private float _turnSpeed = 0;

    [Header("Energy Consumption")]
    [SerializeField] private float _replenishAmount = 1f;
    [SerializeField] private float _consumeAmount = 0.3f;

    private Rigidbody _rbPlayer;
    private float _horizontal = 0;
    private float _vertical = 0;

    private void Awake() => Initial();

    private void Initial() => _rbPlayer = GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
       IsMoving();
    }

    public void JoystickMovement()
    {
        _animator.SetBool(Consts.PlayerAnimations.WALKING,true);
        _horizontal = dynamicJoystick.Horizontal;
        _vertical = dynamicJoystick.Vertical;

        if(EnergyBar.Instance.GetCurrentEnergy() <= 0) 
        {
            Vector3 newPos = new Vector3(_horizontal * _walkSpeed * Time.fixedDeltaTime, 0, _vertical * _walkSpeed * Time.fixedDeltaTime);
            _rbPlayer.linearVelocity = newPos;
        }
        else
        {
            Vector3 newPos = new Vector3(_horizontal * _runSpeed * Time.fixedDeltaTime, 0, _vertical * _runSpeed * Time.fixedDeltaTime);
            _rbPlayer.linearVelocity = newPos;
        }

        Vector3 dir = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _turnSpeed * Time.fixedDeltaTime);
    }

    public void IsMoving()
    {
        if(Input.touchCount > 0)
        {
            JoystickMovement();
            _isMoving = true;
            EnergyBar.Instance.ConsumeEnergy(_consumeAmount);
        }
        else
        {
            _animator.SetBool(Consts.PlayerAnimations.WALKING, false);
            _rbPlayer.linearVelocity = Vector3.zero;
            _isMoving = false;
            EnergyBar.Instance.ReplenishEnergy(_replenishAmount);
        }
        PlayerController.Instance.MovingFX(_isMoving);
    }
}
