using UnityEngine;

public class SensorSprite : MonoBehaviour
{
    [SerializeField] private float _viewRadius = 1.35f;
    [SerializeField] private SpriteRenderer _visionConeSprite;

    void Start()
    {
        if (_visionConeSprite != null)
            UpdateSpriteSize();
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