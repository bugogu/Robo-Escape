using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class LaserGate : MonoBehaviour
{
    #region References

    [Header("General")]
    [SerializeField] private CheckForLaserGateVisibilty _checkForLaserGateVisibilty;
    [SerializeField] private GameObject _lasersParent, _interactionPlate;
    [SerializeField] private float _deactiveTime = 5f;
    [SerializeField] private Transform _reloadImageParent;
    [SerializeField] private Image _reloadFill;
    [SerializeField] private float _consumeEnergyAmount = 50f;

    [Header("Movement")]
    [SerializeField] private bool _canMove = false;
    [SerializeField] Transform _startPoint, _endPoint;
    [SerializeField] float _speed = 1.0f;

    #endregion

    #region Fields

    private float _passedTime = 0f;

    private InteractionPlate _interactionPlateScript;
    private BoxCollider _boxCollider;

    #endregion

    #region Unity Events

    void Start()
    {
        _interactionPlateScript = _interactionPlate.GetComponent<InteractionPlate>();
        _boxCollider = GetComponent<BoxCollider>();

        if (_canMove)
        {
            _reloadImageParent.parent = null;
            _startPoint.parent = null; _endPoint.parent = null;
        }

    }

    void Update()
    {
        if (!_canMove) return;
        Move();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!Player.PlayerController.Instance.IsProtectionActive)
        {
            if (!other.CompareTag(Consts.Tags.PLAYER)) return;

            CameraShake.Shake();

            if (Settings.Instance.Sound == 1) SoundManager.Instance.PlaySFX(SoundManager.Instance.ElectricSfx);

            if (Settings.Instance.Haptic == 1) Handheld.Vibrate();

            EnergyBar.Instance.ConsumeEnergy(_consumeEnergyAmount, true);

            SensorArea[] allSensors = FindObjectsByType<SensorArea>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var sensor in allSensors)
                sensor.SetTriggeredColor();
        }

        GameManager.Instance.SetAlarm(true);
    }

    #endregion

    public void DeactivateLasers()
    {
        SetLasersActive(false);
        DetachFromParent();
        ScheduleLaserActivation();
        AnimateReloadFill();
        DisableCollider();
    }

    #region Private Methods

    private void ActivateLasers()
    {
        SetLasersActive(true);
        AttachToInteractionPlate();
        ResetInteractionPlate();
        EnableCollider();
    }

    private void SetLasersActive(bool isActive) => _lasersParent.SetActive(isActive);

    private void DetachFromParent() => transform.parent = null;

    private void ScheduleLaserActivation() => Invoke(nameof(ActivateLasers), _deactiveTime);

    private void AnimateReloadFill() =>
        _reloadFill.FillImageAnimation(0f, 1f, _deactiveTime)
            .SetEase(Ease.Linear)
            .OnComplete(ResetReloadFill);

    private void ResetReloadFill() => _reloadFill.fillAmount = 0f;

    private void DisableCollider() => _boxCollider.enabled = false;

    private void AttachToInteractionPlate()
    {
        transform.SetParent(_interactionPlate.transform);
        _interactionPlate.SetActive(true);
    }

    private void ResetInteractionPlate()
    {
        _interactionPlateScript.IsInteractionComplete = false;
        _interactionPlateScript.PlateFillImage.fillAmount = 0f;
    }

    private void EnableCollider() => _boxCollider.enabled = true;

    private void Move()
    {
        if (!_checkForLaserGateVisibilty.IsVisibleToCamera()) return;

        _passedTime += _speed * Time.deltaTime;
        var lerpValue = Mathf.PingPong(_passedTime, 1.0f);
        transform.position = Vector3.Lerp(_startPoint.position, _endPoint.position, lerpValue);
    }

    #endregion
}