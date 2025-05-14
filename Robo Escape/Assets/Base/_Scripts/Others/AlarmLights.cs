using UnityEngine;

public class AlarmLights : MonoBehaviour
{
    [SerializeField] private float _alarmLightIntensity = -30f;
    
    private Material _material;
    private float _initialValue = -3f;
    private bool _isAlarmActive = false;
    
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;    
        _initialValue = _material.GetFloat("_Shades");
    }

    void OnEnable()
    {
        GameManager.Instance.OnAlarmSetted += AlarmLightsEnabled;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAlarmSetted -= AlarmLightsEnabled;
    }

    private void AlarmLightsEnabled(bool status)
    {
        if(_isAlarmActive == status) return;
        
        _isAlarmActive = status;
        _material.SetFloat("_Shades", status ? _alarmLightIntensity : _initialValue);
    }
}