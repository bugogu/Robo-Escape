using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField] Color[] _colors;

    void Awake()
    {
        int randomIndex = Random.Range(0, _colors.Length);
        Color randomColor = _colors[randomIndex];

        GetComponent<Camera>().backgroundColor = randomColor;
    }
}
