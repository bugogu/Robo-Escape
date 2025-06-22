namespace Player
{
    using UnityEngine;

    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField] private GameDesignData _gameDesignData;
        [SerializeField] private BoxCollider _elevatorFirstTrigger;

        #region Private Fields

        private PlayerMovement _playerMovement;
        private PlayerController _playerController;
        private IInteractable _currentInteractable;
        private float _interactionTimer, _initialAnimatorSpeed;

        #endregion

        #region Unity Events

        void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerController = GetComponent<PlayerController>();
            _initialAnimatorSpeed = GetComponent<Animator>().speed;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
                BeginInteraction(interactable);

            switch (other.tag)
            {
                case Consts.Tags.MAGNETIC_AREA:

                    MagneticAreaInteraction();

                    break;

                case Consts.Tags.ELEVATOR:

                    ElevatorInteraction(other.GetComponent<Animator>());

                    break;

                case Consts.Tags.FIRST_ELEVATOR_TRIGGER:

                    ElevatorInteraction(other.transform.GetChild(0).GetComponent<Animator>(), true);

                    break;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (_currentInteractable != null)
            {
                _interactionTimer += Time.deltaTime;
                _currentInteractable.OnInteractionStay(_interactionTimer);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Consts.Tags.MAGNETIC_AREA))
            {
                _playerMovement.IsMagnetized = false;
                GetComponent<Animator>().speed = _initialAnimatorSpeed;
            }

            if (_currentInteractable != null)
            {
                _playerController.HackFxActive(_currentInteractable.InteractionType, false);
                _currentInteractable.OnInteractionExit();
                _currentInteractable = null;
                _interactionTimer = 0f;
            }
        }

        #endregion

        #region Private Methods

        private void MagneticAreaInteraction()
        {
            if (_playerController.GetFlashStatus()) return;

            CameraShake.Shake();

            GetComponent<Animator>().speed /= _gameDesignData.MagnetismSpeedMultiplier;
            if (Settings.Instance.Haptic == 1) Handheld.Vibrate();
            _playerMovement.IsMagnetized = true;
        }

        private void ElevatorInteraction(Animator elevatorAnimation, bool first = false)
        {
            if (first)
            {
                _elevatorFirstTrigger.enabled = false;
                elevatorAnimation.SetTrigger(Consts.AnimationParameters.OPEN_ELEVATOR);

                return;
            }

            GameManager.Instance.ChangeGameState(GameState.Win);
            elevatorAnimation.GetComponent<Animator>().SetTrigger(Consts.AnimationParameters.CLOSE_ELEVATOR);
            GetComponent<Animator>().enabled = false;
            _playerMovement.StopMovement();
        }

        private void BeginInteraction(IInteractable interactable)
        {
            _currentInteractable = interactable;
            _currentInteractable.OnInteractionTrigger();
            _playerController.HackFxActive(_currentInteractable.InteractionType, true);
            _interactionTimer = 0f;
        }

        #endregion
    }
}