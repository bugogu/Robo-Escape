using UnityEngine;

public class SensorCamera : MonoBehaviour
{
    [SerializeField] private CheckForCameraVisibilty _checkForCameraVisibilty;

    [Header("Dönüş Ayarları")]
    public bool rotateCamera = true;
    public float rotationSpeed = 30f;
    public float minAngle = -90f; // Sol dönüş açısı
    public float maxAngle = 90f; // Sağ dönüş açısı

    private float currentAngle = 0f;
    private int rotationDirection = 1;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (rotateCamera && !GameManager.Instance.isAlarmActive)
        {
            RotateCamera();
        }
    }

    void RotateCamera()
    {
        if(!_checkForCameraVisibilty.IsVisibleToCamera()) return;

        float rotationAmount = rotationSpeed * rotationDirection * Time.deltaTime;
        currentAngle += rotationAmount;
        
        // Açı limit kontrolü
        if (currentAngle >= maxAngle || currentAngle <= minAngle)
        {
            rotationDirection *= -1;
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
        }
        
        transform.rotation = startRotation * Quaternion.Euler(0, currentAngle, 0);
    }
}
