namespace Player
{
    using UnityEngine;

    public class PlayerInteractions : MonoBehaviour
    {
        [SerializeField] private GameDesignData _gameDesignData;

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
            if (other.TryGetComponent(out ICollectable collectable))
            {
                var fxPlay = collectable.Collect() ? _playerController.ParticleReferences.EnergyCellFX : _playerController.ParticleReferences.DrainCellFX;
                fxPlay.Play();
            }

            if (other.TryGetComponent(out IInteractable interactable))
            {
                _currentInteractable = interactable;
                _currentInteractable.OnInteractionTrigger();
                _playerController.HackFxActive(_currentInteractable.InteractionType, true);
                _interactionTimer = 0f;
            }

            switch (other.tag)
            {
                case Consts.Tags.MAGNETIC_AREA:

                    if (_playerController.GetFlashStatus()) return;

                    CameraShake.Shake();

                    GetComponent<Animator>().speed /= _gameDesignData.MagnetismSpeedMultiplier;

                    if (Settings.Instance.Haptic == 1) Handheld.Vibrate();
                    _playerMovement.IsMagnetized = true;

                    break;

                case Consts.Tags.ELEVATOR:

                    GameManager.Instance.ChangeGameState(GameState.Win);
                    other.GetComponent<Animator>().SetTrigger(Consts.AnimationParameters.CLOSEELEVATOR);
                    GetComponent<Animator>().enabled = false;
                    _playerMovement.StopMovement();

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
    }
}