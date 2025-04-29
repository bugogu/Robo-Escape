using UnityEngine;

public class BackgroundColorChanger : MonoBehaviour
{
    public Color[] colors;

    void Awake()
    {
        int randomIndex = Random.Range(0, colors.Length);
        Color randomColor = colors[randomIndex];

        GetComponent<Camera>().backgroundColor = randomColor;
    }
}
