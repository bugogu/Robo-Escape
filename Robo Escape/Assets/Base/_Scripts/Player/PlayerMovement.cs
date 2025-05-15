namespace Player
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [HideInInspector] public bool IsMoving;
        [HideInInspector] public bool IsMagnetized = false;
        [HideInInspector] public bool OnGround;

        #region References

        [Header("References")]
        [SerializeField] private DynamicJoystick _dynamicJoystick;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameDesignData _gameDesignData;

        #endregion

        #region Private Fields

        private float _runSpeed, _walkSpeed, _turnSpeed;
        private float _magnetismSpeedMultiplier;
        private float _replenishAmount, _consumeAmount, _magnetismConsumptionMultiplier, _magnetismReplenishMultiplier;
        private float _flashSpeedMultiplier, _initialRunSpeed, _initialWalkSpeed;
        private float _movementThreshold, _stopThreshold;
        private Rigidbody _rbPlayer;
        private float _horizontal = 0, _vertical = 0;
        private PlayerController _playerController;

        #endregion

        private void Awake() => Initial();

        private void FixedUpdate()
        {
            if (!OnGround) return;
            PlayerMoving();
        }

        public void SetSpeed(bool isFlashed)
        {
            if (isFlashed)
            {
                _runSpeed *= _flashSpeedMultiplier;
                _walkSpeed *= _flashSpeedMultiplier;
            }
            else
            {
                _walkSpeed = _initialWalkSpeed;
                _runSpeed = _initialRunSpeed;
            }
        }

        public void StopMovement()
        {
            _rbPlayer.linearVelocity = Vector3.zero;
            IsMoving = false;
        }

        #region Private Methods

        private void Initial()
        {
            SetReferecnes();

            OnGround = true;

            _rbPlayer = GetComponent<Rigidbody>();
            _playerController = GetComponent<PlayerController>();

            _flashSpeedMultiplier = _gameDesignData.FlashSpeedMultiplier;

            _initialRunSpeed = _runSpeed;
            _initialWalkSpeed = _walkSpeed;
        }

        private bool JoystickMovement()
        {

            _horizontal = _dynamicJoystick.Horizontal;
            _vertical = _dynamicJoystick.Vertical;

            bool isJoystickMoving = Mathf.Abs(_horizontal) > _movementThreshold || Mathf.Abs(_vertical) > _movementThreshold;

            if (isJoystickMoving)
            {
                _animator.SetBool(Consts.PlayerAnimations.WALKING, isJoystickMoving);

                if (EnergyBar.Instance.GetCurrentEnergy() <= 0)
                {
                    Vector3 newPos = new Vector3(_horizontal * (IsMagnetized ? _walkSpeed / _magnetismSpeedMultiplier : _walkSpeed) * Time.fixedDeltaTime,
                     0, _vertical * (IsMagnetized ? _walkSpeed / _magnetismSpeedMultiplier : _walkSpeed) * Time.fixedDeltaTime);
                    _rbPlayer.linearVelocity = newPos;
                }
                else
                {
                    Vector3 newPos = new Vector3(_horizontal * (IsMagnetized ? _runSpeed / _magnetismSpeedMultiplier : _runSpeed) * Time.fixedDeltaTime,
                     0, _vertical * (IsMagnetized ? _runSpeed / _magnetismSpeedMultiplier : _runSpeed) * Time.fixedDeltaTime);
                    _rbPlayer.linearVelocity = newPos;
                }

                Vector3 dir = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _turnSpeed * Time.fixedDeltaTime);
            }
            else
            {
                if (Mathf.Abs(_horizontal) < _stopThreshold || Mathf.Abs(_vertical) < _stopThreshold)
                {
                    return false;
                }
            }

            return isJoystickMoving;
        }

        private void PlayerMoving()
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                if (JoystickMovement())
                {
                    IsMoving = true;

                    if (!_playerController.GetFlashStatus())
                        EnergyBar.Instance.ConsumeEnergy(IsMagnetized ? _magnetismConsumptionMultiplier * _consumeAmount : _consumeAmount);
                }
                else
                {
                    _animator.SetBool(Consts.PlayerAnimations.WALKING, false);
                    StopMovement();
                    EnergyBar.Instance.ReplenishEnergy(IsMagnetized ? _magnetismReplenishMultiplier * _replenishAmount : _replenishAmount);
                }
            }
            else
            {
                _animator.SetBool(Consts.PlayerAnimations.WALKING, false);
                StopMovement();
                EnergyBar.Instance.ReplenishEnergy(IsMagnetized ? _magnetismReplenishMultiplier * _replenishAmount : _replenishAmount);
            }
            PlayerController.Instance.MovingFX(IsMoving);
        }

        private void SetReferecnes()
        {
            _runSpeed = _gameDesignData.CharacterRunSpeed;
            _walkSpeed = _gameDesignData.CharacterWalkSpeed;
            _turnSpeed = _gameDesignData.CharacterTurnSpeed;
            _magnetismSpeedMultiplier = _gameDesignData.MagnetismSpeedMultiplier;

            _replenishAmount = _gameDesignData.ReplenishAmount;
            _consumeAmount = _gameDesignData.ConsumeAmount;
            _magnetismConsumptionMultiplier = _gameDesignData.MagnetismConsumptionMultiplier;
            _magnetismReplenishMultiplier = _gameDesignData.MagnetismReplenishMultiplier;

            _movementThreshold = _gameDesignData.MovementThreshold;
            _stopThreshold = _gameDesignData.StopThreshold;
        }

        #endregion
    }
}