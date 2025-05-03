using UnityEngine;

public class SensorSprite : MonoBehaviour
{
    [Header("Görüş Ayarları")]
    [SerializeField] private float viewRadius = 7f;

    [Header("Sprite Ayarları")]
    [SerializeField] private SpriteRenderer visionConeSprite;

    [Header("Dönüş Ayarları")]
    public bool rotateCamera = true;
    public float rotationSpeed = 30f;
    public float minAngle = -90f; // Sol dönüş açısı
    public float maxAngle = 90f; // Sağ dönüş açısı

    private int rotationDirection = -1;
    private float currentAngle = 0f;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;

        if (visionConeSprite != null)
        {
            UpdateSpriteSize();
        }
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
        float rotationAmount = rotationSpeed * Time.deltaTime * rotationDirection;
        currentAngle += rotationAmount;
        
        // Açı limit kontrolü
        if (currentAngle >= maxAngle || currentAngle <= minAngle)
        {
            rotationDirection *= -1;
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
        }
        
        transform.rotation = startRotation * Quaternion.Euler(0, currentAngle, 0);
    }

    void UpdateSpriteSize()
    {
        if (visionConeSprite != null)
        {
            visionConeSprite.transform.localScale = new Vector3(viewRadius * 2, viewRadius * 2, 1);
        }
    }

    void OnValidate()
    {
        UpdateSpriteSize();
    }
}
