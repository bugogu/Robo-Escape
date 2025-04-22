using UnityEngine;

public class SensorArea : MonoBehaviour
{
    [SerializeField] private Color _triggeredColor;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Consts.Tags.PLAYER))
        {
            if(Settings.Instance.Haptic == 1) Handheld.Vibrate();
            SensorArea[] allSensors = FindObjectsByType<SensorArea>(FindObjectsInactive.Include, FindObjectsSortMode.None);
             
            foreach (var sensor in allSensors)
            {
                sensor.SetTriggeredColor();
            }

            GameManager.Instance.SetAlarm(true);
        }
    }

    public void SetTriggeredColor()
    {
        _spriteRenderer.color = _triggeredColor;
    }
}
