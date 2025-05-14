using UnityEngine;

public class SensorSprite : MonoBehaviour
{
    #region References

    [Header("Rotation Settings")]
    [SerializeField] bool _rotateSprite = true;
    [SerializeField] float _rotationSpeed = 45f, _minAngle = -70f, _maxAngle = 70f;

    [Header("Others")]
    [SerializeField] private CheckForCameraVisibilty _checkForCameraVisibilty;
    [SerializeField] private float _viewRadius = 1.35f;
    [SerializeField] private SpriteRenderer _visionConeSprite;

    #endregion

    private int _rotationDirection = -1;
    private float _currentAngle = 0f;
    private Quaternion _startRotation;

    void Start()
    {
        _startRotation = transform.rotation;

        if (_visionConeSprite != null)
        {
            UpdateSpriteSize();
        }
    }

    void Update()
    {
        if (_rotateSprite && !GameManager.Instance.IsAlarmActive)
        {
            RotateCamera();
        }
    }

    void RotateCamera()
    {
        if(!_checkForCameraVisibilty.IsVisibleToCamera()) return;

        float rotationAmount = _rotationSpeed * _rotationDirection * Time.deltaTime;
        _currentAngle += rotationAmount;
        
        if (_currentAngle >= _maxAngle || _currentAngle <= _minAngle)
        {
            _rotationDirection *= -1;
            _currentAngle = Mathf.Clamp(_currentAngle, _minAngle, _maxAngle);
        }
        
        transform.rotation = _startRotation * Quaternion.Euler(0, _currentAngle, 0);
    }

    void UpdateSpriteSize()
    {
        if (_visionConeSprite != null)
        {
            _visionConeSprite.transform.localScale = new Vector3(_viewRadius * 2, _viewRadius * 2, 1);
        }
    }

    void OnValidate()
    {
        UpdateSpriteSize();
    }
}