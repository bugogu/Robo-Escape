using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    public LevelDesignData levelData;
    [HideInInspector] public int chipsetCount;
    [HideInInspector]
    [Tooltip("Seconds")] public float _timeLimit;
    public bool timeMissionCompleted => _passedTime < _timeLimit;
    public bool alarmMissionCompleted => !GameManager.Instance.isAlarmActive;
    public bool chipsetMissionCompleted => IsNessaryChipsetCollected();

    [SerializeField] private Light _directionalLight;
    
    private int _collectedChipsetCount = 0;
    private float _passedTime;
    private bool _levelStarted = false;

    public void CollectChipset() => _collectedChipsetCount++;

    private bool IsNessaryChipsetCollected() => _collectedChipsetCount >= chipsetCount;

    void Awake()
    {
        chipsetCount = levelData.chipsetCount;
        _timeLimit = levelData.timeLimit;
    }

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += IncreaseProtocolCount;
        GameManager.Instance.OnGameStateChanged += StartTimer;
        GameManager.Instance.OnAlarmSetted += ChangeTemperature;
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= IncreaseProtocolCount;
        GameManager.Instance.OnGameStateChanged -= StartTimer;
        GameManager.Instance.OnAlarmSetted -= ChangeTemperature;
    }

    private void IncreaseProtocolCount(GameState gameState)
    {
        if(gameState != GameState.Win) return;

        if(IsNessaryChipsetCollected()) GameManager.Instance.ProtocolCount++;

        if(!GameManager.Instance.isAlarmActive) GameManager.Instance.ProtocolCount++;

        if(_passedTime < _timeLimit) GameManager.Instance.ProtocolCount++;
    }

    void Update()
    {
        if(!_levelStarted) return;

        _passedTime += Time.deltaTime / _timeLimit;
    }

    private void StartTimer(GameState gameState)
    {
        if(gameState != GameState.Intro) _levelStarted = true;
        else _levelStarted = false;
    }

    private void ChangeTemperature(bool status)
    {
        _directionalLight.useColorTemperature = status;
        _directionalLight.colorTemperature = 1500f;
    }
}
