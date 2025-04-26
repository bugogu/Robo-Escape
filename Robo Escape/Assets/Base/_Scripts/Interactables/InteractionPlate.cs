using UnityEngine.UI;
using UnityEngine;

public class InteractionPlate : MonoBehaviour, IInteractable
{
    public bool isPowerUpPlate = false;
    public bool oneTimeUseable = true;

    [SerializeField] private UnityEngine.Events.UnityEvent _action;
    [Space(10)]
    [SerializeField] private bool _visualCanHide = false;
    [SerializeField] private GameObject _plateVisualObject;
    [SerializeField] private InteractionType _interactionType ;
    [SerializeField] private float _interactionDuration = 5f;

    public Image _plateFillImage;
    public InteractionType InteractionType => _interactionType;

    [HideInInspector]
    public bool _isInteractionComplete = false;

    void Awake()
    {
        if(_visualCanHide)
        _plateVisualObject.SetActive(false);
    }

    public void OnInteractionTrigger()
    {
        if(_visualCanHide)
        _plateVisualObject.SetActive(true);
    }

    public void OnInteractionStay(float duration)
    {
        if(isPowerUpPlate)
            if(PlayerController.Instance._hasAnyPowerUp) return;

            if(_isInteractionComplete) return;

            _plateFillImage.fillAmount = Mathf.Clamp01(duration / Mathf.Max(0.001f, _interactionDuration));

             if (duration >= _interactionDuration) 
            {
                FindAnyObjectByType<PlayerController>().HackFxActive(_interactionType, false);
                _action.RemoveAllListeners();
                _action?.Invoke();

                if(oneTimeUseable)
                {
                    gameObject.SetActive(false);
                    _isInteractionComplete = true;  
                }
            }
    }

    public void OnInteractionExit()
    {
        if(_visualCanHide)
        _plateVisualObject.SetActive(false);

        _plateFillImage.fillAmount = 0f;
    }
}

