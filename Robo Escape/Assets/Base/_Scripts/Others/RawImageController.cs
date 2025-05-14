using UnityEngine;
using UnityEngine.UI;

public class RawImageController : MonoBehaviour
{
    [SerializeField] float _scrollSpeedX = -0.1f; 
    [SerializeField] float _scrollSpeedY = -0.1f; 

    private Material _material;
    private RawImage _rawImage;

    void Start()
    {
        _rawImage = GetComponent<RawImage>();

        if (_rawImage != null)
            _material = _rawImage.material;
    }

    void Update()
    {
        if (_material != null)
        {
            Vector2 offset = _material.mainTextureOffset;
            offset.x += _scrollSpeedX * Time.deltaTime;
            offset.y += _scrollSpeedY * Time.deltaTime;
            _material.mainTextureOffset = offset;
        }
    }
}