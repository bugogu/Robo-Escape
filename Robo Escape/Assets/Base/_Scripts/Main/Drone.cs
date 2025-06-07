using UnityEngine;

public class Drone : MonoBehaviour, IEffectorForProp
{
    #region References

    [SerializeField] private VisionCone _visionCone;

    #endregion

    #region Private Fields

    private bool _isPatroling = true;
    private CheckForDroneVisibilty _checkForDroneVisibilty;

    #endregion

    #region Unity Events

    void Awake()
    {
        _checkForDroneVisibilty = transform.GetChild(0).GetComponent<CheckForDroneVisibilty>();
    }

    void Update()
    {
        if (!_checkForDroneVisibilty.IsVisibleToCamera())
        {
            _visionCone.enabled = false;
            return;
        }
        else
        {
            _visionCone.enabled = true;
        }
    }

    void OnEnable()
    {
        GameManager.Instance.OnAlarmSetted += StopPatroling;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAlarmSetted -= StopPatroling;
    }

    #endregion

    public void PlayerSpotted()
    {
        GameManager.Instance.SetAlarm(true);
    }

    public void StopPatroling(bool status)
    {
        _isPatroling = false;
    }
}