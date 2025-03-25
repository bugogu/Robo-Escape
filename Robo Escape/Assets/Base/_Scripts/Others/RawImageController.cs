using UnityEngine;
using UnityEngine.UI;

public class RawImageController : MonoBehaviour
{
    [SerializeField] RawImage rawImage; // Raw Image bileşeni
    public float scrollSpeedX = 0.1f; // X ekseninde kayma hızı
    public float scrollSpeedY = 0.1f; // Y ekseninde kayma hızı

    private Material material;

    void Start()
    {
        if (rawImage != null)
        {
            material = rawImage.material;
        }
    }

    void Update()
    {
        if (material != null)
        {
            Vector2 offset = material.mainTextureOffset;
            offset.x += scrollSpeedX * Time.deltaTime;
            offset.y += scrollSpeedY * Time.deltaTime;
            material.mainTextureOffset = offset;
        }
    }
}
