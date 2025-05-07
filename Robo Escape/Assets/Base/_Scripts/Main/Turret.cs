using UnityEngine;
using DG.Tweening;

public class Turret : MonoBehaviour, IEffectableFromEMP
{
    [SerializeField] private float spottedRotationSpeed = 10f;
    [SerializeField] private VisionCone _visionCone;
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _projectileEnergyConsumptionAmount = 10f;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private ParticleSystem _projectileFireEffect;

    [Header("Recoil Settings")]
    [SerializeField] private float recoilDistance = -0.5f; // Geri tepme mesafesi
    [SerializeField] private float recoilDuration = 0.1f; // Geri tepme süresi
    [SerializeField] private float returnDuration = 0.3f; // Başlangıç pozisyonuna dönüş süresi
    [SerializeField] private Ease recoilEase = Ease.OutQuad; // Geri tepme eğrisi
    [SerializeField] private Ease returnEase = Ease.OutElastic; // Geri dönüş eğrisi

    [Header("Dönüş Ayarları")]
    public bool rotateTurret = true;
    public float rotationSpeed = 30f;
    public float minAngle = -90f; 
    public float maxAngle = 90f; 

    private float currentAngle = 0f;
    private int rotationDirection = 1;
    private Quaternion startRotation;
    private float _nextFireTime = 0f;
    private Camera _mainCamera;
    private Renderer _renderer;
    private Vector3 _originalPosition;
    private Sequence _recoilSequence;
    private AudioSource _audioSource;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        startRotation = transform.rotation;
        currentAngle = startRotation.eulerAngles.y; 
        _mainCamera = Camera.main;
        _renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if(!IsVisibleToCamera()) _visionCone.enabled = false;
        if(rotateTurret && IsVisibleToCamera())
        {
            _visionCone.enabled = true;
            RotateTurret();
        } 
    }

    public void PlayerSpotted(Transform player) 
    {
        if(!_visionCone.playerSpotted) return;

        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + _fireRate;
            Fire(player); 
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * spottedRotationSpeed);

            currentAngle = angle;
        }
    }

    void RotateTurret()
    {   
        if(_visionCone.playerSpotted) return;

            currentAngle = transform.eulerAngles.y;
        if (currentAngle > 180f) currentAngle -= 360f;

        float rotationAmount = rotationSpeed * Time.deltaTime * rotationDirection;
        currentAngle += rotationAmount;
    
        if (currentAngle >= maxAngle || currentAngle <= minAngle)
        {
            rotationDirection *= -1;
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
        }
    
        transform.rotation = Quaternion.Euler(0, currentAngle, 0);
    }

    // Projectile lar burdan spawnlanıcak
    private void Fire(Transform target)
    {
        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);

        projectile.GetComponent<Projectile>().damage = _projectileEnergyConsumptionAmount;
    
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        _projectileFireEffect?.Play();

        if(Settings.Instance.Sound == 1) _audioSource?.Play();

        FireRecoil();

        if (rb != null && target != null)
        {
            Vector3 direction = (target.position - _firePoint.position).normalized;
            rb.AddForce((direction + Vector3.up / 10)  * _projectileSpeed, ForceMode.Impulse);

            projectile.transform.SetParent(null);
        }
    }

    bool IsVisibleToCamera()
    {
        if (_renderer == null || _mainCamera == null)
            return false;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_mainCamera);
        return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
    }

    public void FireRecoil()
    {
        // Önceki animasyon varsa durdur
        if (_recoilSequence != null && _recoilSequence.IsActive())
        {
            _recoilSequence.Kill();
        }

        // GÜNCEL YÖNE GÖRE GERİ TEPME VEKTÖRÜ
        // Nesnenin kendi ileri yönünün tersi yönünde geri tepme
        Vector3 recoilDirection = transform.forward;
        
        // Eğer parent varsa, local space'e çevir
        if (transform.parent != null)
        {
            recoilDirection = transform.parent.InverseTransformDirection(recoilDirection);
        }

        // DOTween Sequence oluştur
        _recoilSequence = DOTween.Sequence();
        
        // 1. GERİ TEPME AŞAMASI
        _recoilSequence.Append(
            transform.DOLocalMove(
                _originalPosition + recoilDirection * recoilDistance,
                recoilDuration
            )
            .SetEase(recoilEase)
        );
        
        // 2. BAŞLANGIÇ POZİSYONUNA DÖNÜŞ
        _recoilSequence.Append(
            transform.DOLocalMove(
                _originalPosition,
                returnDuration
            )
            .SetEase(returnEase)
        );
    }

    public void EffectFromEMP()
    {
        
    }
}
