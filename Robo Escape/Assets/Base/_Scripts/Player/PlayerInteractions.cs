using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private GameDesignData _gameDesignData;
    private PlayerMovement _playerMovement;
    private PlayerController _playerController;

    private IInteractable _currentInteractable;
    private float _interactionTimer;
    private float _initialAnimatorSpeed;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
        _initialAnimatorSpeed = GetComponent<Animator>().speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICollectable collectable)) 
        {
            var fxPlay = collectable.Collect() ? _playerController._energyCellFX : _playerController._drainCellFX;
            fxPlay.Play();
        }

        if(other.CompareTag(Consts.Tags.MAGNETIC_AREA))
        {
            if(_playerController.GetFlashStatus()) return;

            GetComponent<Animator>().speed /= _gameDesignData.magnetismSpeedMultiplier;

            if(Settings.Instance.Haptic == 1) Handheld.Vibrate();
            _playerMovement._isMagnetized = true;
        } 

        if(other.CompareTag(Consts.Tags.ELEVATOR))
        {
            GameManager.Instance.ChangeGameState(GameState.Win);
            other.GetComponent<Animator>().SetTrigger(Consts.AnimationParameters.CLOSEELEVATOR);
            GetComponent<Animator>().enabled = false;
            _playerMovement.StopMovement();
        }
           
        if (other.TryGetComponent(out IInteractable interactable))
        {
            _currentInteractable = interactable;
            _currentInteractable.OnInteractionTrigger();
            _playerController.HackFxActive(_currentInteractable.InteractionType, true);
            _interactionTimer = 0f;
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
        if(other.CompareTag(Consts.Tags.MAGNETIC_AREA)) 
        {
            _playerMovement._isMagnetized = false;
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
}
