using UnityEngine;

public class SensorCamera : MonoBehaviour
{
    #region References

    [SerializeField] private CheckForCameraVisibilty _checkForCameraVisibilty;

    [Header("Rotation Settings")]
    [SerializeField] bool _rotateCamera = true;
    [SerializeField] float _rotationSpeed = 45f, _minAngle = -70f, _maxAngle = 70f;
    
    #endregion

    private float _currentAngle = 0f;
    private int _rotationDirection = 1;
    private Quaternion _startRotation;

    void Start() =>
        _startRotation = transform.rotation;

    void Update()
    {
        if (_rotateCamera && !GameManager.Instance.IsAlarmActive)
            RotateCamera();
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

        transform.rotation = Quaternion.Euler(
        transform.rotation.eulerAngles.x, 
        _currentAngle,                     
        transform.rotation.eulerAngles.z   
        );
    }
}