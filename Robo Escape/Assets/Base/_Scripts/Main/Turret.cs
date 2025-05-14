using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Turret : MonoBehaviour, IEffectableFromEMP
{
    #region References

    [Header("Floats")]
    [SerializeField] private float _spottedRotationSpeed = 10f;
    [SerializeField] private float _projectileEnergyConsumptionAmount = 10f, _fireRate = 2f, _empEffectDuration = 3f, _projectileSpeed;

    [Header("Recoil Settings")]
    [SerializeField] private float _recoilDistance = -0.5f;
    [SerializeField] private float _recoilDuration = 0.1f;
    [SerializeField] private float _returnDuration = 0.3f;
    [SerializeField] private Ease _recoilEase = Ease.OutQuad;
    [SerializeField] private Ease _returnEase = Ease.OutElastic;

    [Header("Rotation Settings")]
    [SerializeField] bool _rotateTurret = true;
    [SerializeField] float _rotationSpeed = 15f;
    [SerializeField] float _minAngle = -45f; 
    [SerializeField] float _maxAngle = 45f; 

    [Header("Others")]
    [SerializeField] private VisionCone _visionCone;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private ParticleSystem _projectileFireEffect;
    [SerializeField] private Image _empDurationFillImage;
    [SerializeField] private Material _turretActiveMaterial, _turretInactiveMaterial;

    #endregion

    #region Private Fields

    private float _currentAngle = 0f;
    private int _rotationDirection = 1;
    private Quaternion _startRotation;
    private float _nextFireTime = 0f;
    private Camera _mainCamera;
    private Renderer _renderer;
    private Vector3 _originalPosition;
    private Sequence _recoilSequence;
    private AudioSource _audioSource;
    private bool _isEffected = false;

    #endregion

    #region Unity Events

    private void Awake()
    {
        _originalPosition = transform.localPosition;
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _startRotation = transform.rotation;
        _currentAngle = _startRotation.eulerAngles.y; 
        _mainCamera = Camera.main;
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if(!IsVisibleToCamera()) _visionCone.enabled = false;

        if(_isEffected) return;

        if(_rotateTurret && IsVisibleToCamera())
        {
            _visionCone.enabled = true;
            RotateTurret();
        } 
    }

    #endregion

    #region Public Methods

    public void PlayerSpotted(Transform player) 
    {
        if(!_visionCone.PlayerSpotted) return;

        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + _fireRate;
            Fire(player); 
        }

        var direction = player.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _spottedRotationSpeed);

            _currentAngle = angle;
        }
    }

    public void FireRecoil()
    {
        if (_recoilSequence != null && _recoilSequence.IsActive())
        {
            _recoilSequence.Kill();
        }

        var recoilDirection = transform.forward;
        
        if (transform.parent != null)
        {
            recoilDirection = transform.parent.InverseTransformDirection(recoilDirection);
        }

        _recoilSequence = DOTween.Sequence();
        
        _recoilSequence.Append(
            transform.DOLocalMove(
                _recoilDistance * recoilDirection + _originalPosition,
                _recoilDuration
            )
            .SetEase(_recoilEase)
        );
        
        _recoilSequence.Append(
            transform.DOLocalMove(
                _originalPosition,
                _returnDuration
            )
            .SetEase(_returnEase)
        );
    }

    public void EffectFromEMP()
    {
        _isEffected = true;
        transform.parent.GetComponent<Renderer>().material = _turretInactiveMaterial;
        _visionCone.VisionRange = 0;
        Invoke(nameof(DeactivateVision), 0.2f);
        _empDurationFillImage.transform.parent.gameObject.SetActive(true);
        _empDurationFillImage.FillImageAnimation(0, 1, _empEffectDuration).SetEase(Ease.Linear);
        Invoke(nameof(RemoveEmpEffects), _empEffectDuration);
    }

    #endregion

    #region Private Methods

    private void RotateTurret()
    {   
        if(_visionCone.PlayerSpotted) return;

            _currentAngle = transform.eulerAngles.y;
        if (_currentAngle > 180f) _currentAngle -= 360f;

        float rotationAmount = _rotationSpeed * _rotationDirection * Time.deltaTime;
        _currentAngle += rotationAmount;
    
        if (_currentAngle >= _maxAngle || _currentAngle <= _minAngle)
        {
            _rotationDirection *= -1;
            _currentAngle = Mathf.Clamp(_currentAngle, _minAngle, _maxAngle);
        }
    
        transform.rotation = Quaternion.Euler(0, _currentAngle, 0);
    }

    private void Fire(Transform target)
    {
        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);

        projectile.GetComponent<Projectile>().Damage = _projectileEnergyConsumptionAmount;
    
        var rb = projectile.GetComponent<Rigidbody>();

        _projectileFireEffect?.Play();

        if(Settings.Instance.Sound == 1) _audioSource?.Play();

        FireRecoil();

        if (rb != null && target != null)
        {
            Vector3 direction = (target.position - _firePoint.position).normalized;
            rb.AddForce(_projectileSpeed * (direction + Vector3.up / 10), ForceMode.Impulse);

            projectile.transform.SetParent(null);
        }
    }

    private bool IsVisibleToCamera()
    {
        if (_renderer == null || _mainCamera == null)
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
        return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
    }

    private void RemoveEmpEffects()
    {
        _visionCone.VisionRange = _visionCone.InitialRange;
        transform.parent.GetComponent<Renderer>().material = _turretActiveMaterial;
        _empDurationFillImage.transform.parent.gameObject.SetActive(false);
        _isEffected = false;
        _visionCone.enabled = true;
    }

    private void DeactivateVision() => _visionCone.enabled = false;

    #endregion
}