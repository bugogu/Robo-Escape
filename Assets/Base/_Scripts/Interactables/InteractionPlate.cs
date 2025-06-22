using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;
using Player;

[SelectionBase]
public class InteractionPlate : MonoBehaviour, IInteractable
{
    #region Public Fields

    public bool IsPowerUpPlate = false;
    public bool OneTimeUseable = true;

    [FormerlySerializedAs("_plateFillImage")] public Image PlateFillImage;
    public InteractionType InteractionType => _interactionType;

    [HideInInspector] public bool IsInteractionComplete = false;

    #endregion

    #region References

    [SerializeField] private UnityEngine.Events.UnityEvent _action;
    [Space(10)]
    [SerializeField] private bool _visualCanHide = false;
    [SerializeField] private GameObject _plateVisualObject;
    [SerializeField] private InteractionType _interactionType;
    [SerializeField] private float _interactionDuration = 5f;

    #endregion

    #region Unity Events

    void Awake() => HandleVisualVisibilty(false);

    public void OnInteractionTrigger() =>
        HandleVisualVisibilty(true);

    public void OnInteractionStay(float duration)
    {
        if (IsPowerUpPlate)
            if (PlayerController.Instance.HasAnyPowerUp) return;

        if (IsInteractionComplete) return;

        PlateFillImage.fillAmount = Mathf.Clamp01(duration / Mathf.Max(0.001f, _interactionDuration));

        if (duration >= _interactionDuration)
        {
            FindAnyObjectByType<PlayerController>().HackFxActive(_interactionType, false);
            _action.RemoveAllListeners();
            _action?.Invoke();

            if (Settings.Instance.Sound == 1)
                LevelManager.Instance.PlayHackSFX();

            if (OneTimeUseable)
            {
                gameObject.SetActive(false);
                IsInteractionComplete = true;
            }
        }
    }

    public void OnInteractionExit()
    {
        if (_visualCanHide)
            _plateVisualObject.SetActive(false);

        PlateFillImage.fillAmount = 0f;
    }

    public void ReActive()
    {
        gameObject.SetActive(true);
        IsInteractionComplete = false;
    }

    private void HandleVisualVisibilty(bool status)
    { 
        if (_visualCanHide)
            _plateVisualObject.SetActive(status);
    }

    #endregion
}