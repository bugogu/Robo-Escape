using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private VisionCone _visionCone;
    private bool _isPatroling = true;
    private CheckForDroneVisibilty _checkForDroneVisibilty;

    void Awake()
    {
        _checkForDroneVisibilty = transform.GetChild(0).GetComponent<CheckForDroneVisibilty>();
    }

    void Update()
    {
        if(!_checkForDroneVisibilty.IsVisibleToCamera())
        {
            _visionCone.enabled = false;
            return;
        }
        else
        {
            _visionCone.enabled = true;
        }
    }

    public void PlayerSpotted()
    {
        GameManager.Instance.SetAlarm(true);
    }

    public void StopPatroling(bool status) 
    {
        _isPatroling = false;
    }
        
    void OnEnable()
    {
        GameManager.Instance.OnAlarmSetted += StopPatroling;
    }

    void OnDisable()
    {
        GameManager.Instance.OnAlarmSetted -= StopPatroling;
    }
}
