using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class LaserGate : MonoBehaviour
{
    [SerializeField] private GameObject _lasersParent;
    [SerializeField] private GameObject _interactionPlate;
    [SerializeField] private float _deactiveTime = 5f;
    [SerializeField] private Transform _reloadImageParent;
    [SerializeField] private Image _reloadFill;
    [SerializeField] private float _consumeEnergyAmount = 50f;

    [Header("Movement")]
    [SerializeField] private bool _canMove = false;
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _endPoint;
    [SerializeField] float _speed = 1.0f;

    private float _passedTime = 0f;

    private InteractionPlate _interactionPlateScript;
    private BoxCollider _boxCollider;

    void Start()
    {
        _interactionPlateScript = _interactionPlate.GetComponent<InteractionPlate>();
        _boxCollider = GetComponent<BoxCollider>();

        if(_canMove)
        _reloadImageParent.parent = null;
    }

    void Update()
    {
        if(!_canMove) return;
        Move();
    }

    public void DeactivateLasers()
    {
        _lasersParent.SetActive(false);
         transform.parent = null;
        Invoke(nameof(ActivateLasers), _deactiveTime);
        _reloadFill.FillImageAnimation(0f, 1f, _deactiveTime).SetEase(Ease.Linear).OnComplete(()=> _reloadFill.fillAmount = 0f);
        _reloadImageParent.parent = null;
        _boxCollider.enabled = false;
    }

    private void ActivateLasers()
    {
        _lasersParent.SetActive(true);
        transform.SetParent(_interactionPlate.transform);
        _interactionPlate.SetActive(true);
        _interactionPlateScript._isInteractionComplete = false;
        _interactionPlateScript._plateFillImage.fillAmount = 0f;
        _boxCollider.enabled = true;
    }

    private void Move()
    {
        _passedTime += Time.deltaTime * _speed;
        float lerpDegeri = Mathf.PingPong(_passedTime, 1.0f); 

        transform.position = Vector3.Lerp(_startPoint.position, _endPoint.position, lerpDegeri);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!PlayerController.Instance._isProtectionActive)
        {
            if(!other.CompareTag(Consts.Tags.PLAYER)) return;

            if(Settings.Instance.Haptic == 1) Handheld.Vibrate();

            EnergyBar.Instance.ConsumeEnergy(_consumeEnergyAmount, true); 

            SensorArea[] allSensors = FindObjectsByType<SensorArea>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var sensor in allSensors)
                sensor.SetTriggeredColor();
        }

        GameManager.Instance.SetAlarm(true);
    }
}
