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
        private Vector3 _movementVector;

        #endregion

        private void Awake() => Initial();

        private void FixedUpdate()
        {
            if (!OnGround) return;
            PlayerMovementActions();
        }

        /// <summary>
        /// Sets the speed of the player with flash powerup.
        /// </summary>
        /// <param name="isFlashed"> If set to <c>true</c> is flashed. </param>
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

        /// <summary>
        /// Stops the movement of the player with linearVelocity and changes the public field IsMoving to false.
        /// </summary>
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

            var isMoving = Mathf.Abs(_horizontal) > _movementThreshold || Mathf.Abs(_vertical) > _movementThreshold;
            var isStopping = Mathf.Abs(_horizontal) >= _stopThreshold || Mathf.Abs(_vertical) >= _stopThreshold;

            if (isMoving)
            {
                HandleMovement();
                HandleRotation();
                return true;
            }

            return isStopping;
        }

        private void PlayerMovementActions()
        {
            var isInputActive = Input.touchCount > 0 || Input.GetMouseButton(0);
            var isJoystickMoving = JoystickMovement();

            IsMoving = isInputActive && isJoystickMoving;

            HandleMovementAnimation();
            HandleEnergyConsumption();

            PlayerController.Instance.MovingFX(IsMoving);
        }

        private void HandleMovementAnimation()
        {
            _animator.SetBool(Consts.PlayerAnimations.WALKING, IsMoving);

            if (!IsMoving)
                StopMovement();
        }

        private void HandleEnergyConsumption()
        {
            if (IsMoving && !_playerController.GetFlashStatus())
            {
                var consumption = _consumeAmount * (IsMagnetized ? _magnetismConsumptionMultiplier : 1f);
                EnergyBar.Instance.ConsumeEnergy(consumption);
            }
            else
            {
                var replenish = _replenishAmount * (IsMagnetized ? _magnetismReplenishMultiplier : 1f);
                EnergyBar.Instance.ReplenishEnergy(replenish);
            }
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

        private void HandleMovement()
        {
            var speed = GetCurrentSpeed();
            var speedMultiplier = IsMagnetized ? 1f / _magnetismSpeedMultiplier : 1f;

            _movementVector.x = _horizontal * speed * speedMultiplier * Time.fixedDeltaTime;
            _movementVector.z = _vertical * speed * speedMultiplier * Time.fixedDeltaTime;
            _movementVector.y = 0;

            _rbPlayer.linearVelocity = _movementVector;
        }

        private void HandleRotation()
        {
            var direction = (Vector3.forward * _vertical) + (Vector3.right * _horizontal);

            if (direction == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _turnSpeed * Time.fixedDeltaTime);
        }

        private float GetCurrentSpeed()
        {
            return EnergyBar.Instance.GetCurrentEnergy() > 0 ? _runSpeed : _walkSpeed;
        }

        #endregion
    }
}