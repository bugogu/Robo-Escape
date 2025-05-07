using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float spottedRotationSpeed = 10f;
    [SerializeField] private VisionCone _visionCone;
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _projectileEnergyConsumptionAmount = 10f;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _projectileSpeed;

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

        if (rb != null && target != null)
        {
            Vector3 direction = (target.position - _firePoint.position).normalized;
            rb.AddForce(direction * _projectileSpeed, ForceMode.Impulse);

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
}
