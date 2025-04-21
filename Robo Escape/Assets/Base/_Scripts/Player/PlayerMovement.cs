using UnityEngine;

[RequireComponent ( typeof ( Rigidbody ) )]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public bool _isMoving;
    [HideInInspector] public bool _isMagnetized = false;

    [Header("References")]
    [SerializeField] private DynamicJoystick dynamicJoystick;
    [SerializeField] private Animator _animator;

    [SerializeField] private GameDesignData _gameDesignData;

    // Movement
    private float _runSpeed;
    private float _walkSpeed;
    private float _turnSpeed;
    private float _magnetismSpeedMultiplier;

    // Energy Settings
    private float _replenishAmount;
    private float _consumeAmount;
    private float _magnetismConsumptionMultiplier;
    private float _magnetismReplenishMultiplier;

    private Rigidbody _rbPlayer;
    private float _horizontal = 0;
    private float _vertical = 0;

    private void Awake() => Initial();

    private void Initial() 
    {
        SetReferecnes();

        _rbPlayer = GetComponent<Rigidbody>();
    }

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
            Vector3 newPos = new Vector3(_horizontal * (_isMagnetized ? _walkSpeed / _magnetismSpeedMultiplier : _walkSpeed) * Time.fixedDeltaTime,
             0, _vertical * (_isMagnetized ? _walkSpeed / _magnetismSpeedMultiplier : _walkSpeed) * Time.fixedDeltaTime);
            _rbPlayer.linearVelocity = newPos;
        }
        else
        {
            Vector3 newPos = new Vector3(_horizontal * (_isMagnetized ? _runSpeed / _magnetismSpeedMultiplier : _runSpeed) * Time.fixedDeltaTime,
             0, _vertical * (_isMagnetized ? _runSpeed / _magnetismSpeedMultiplier : _runSpeed) * Time.fixedDeltaTime);
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
            EnergyBar.Instance.ConsumeEnergy(_isMagnetized ? _magnetismConsumptionMultiplier * _consumeAmount : _consumeAmount);
        }
        else
        {
            _animator.SetBool(Consts.PlayerAnimations.WALKING, false);
            _rbPlayer.linearVelocity = Vector3.zero;
            _isMoving = false;
            EnergyBar.Instance.ReplenishEnergy(_isMagnetized ? _magnetismReplenishMultiplier * _replenishAmount : _replenishAmount);
        }
        PlayerController.Instance.MovingFX(_isMoving);
    }

    private void SetReferecnes()
    {
        _runSpeed = _gameDesignData.characterRunSpeed;
        _walkSpeed = _gameDesignData.characterWalkSpeed;
        _turnSpeed = _gameDesignData.characterTurnSpeed;
        _magnetismSpeedMultiplier = _gameDesignData.magnetismSpeedMultiplier;

        _replenishAmount = _gameDesignData.replenishAmount;
        _consumeAmount = _gameDesignData.consumeAmount;
        _magnetismConsumptionMultiplier = _gameDesignData.magnetismConsumptionMultiplier;
        _magnetismReplenishMultiplier = _gameDesignData.magnetismReplenishMultiplier;
    }
}
