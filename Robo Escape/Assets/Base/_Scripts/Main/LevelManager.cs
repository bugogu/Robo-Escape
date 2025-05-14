using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    public LevelDesignData LevelData;

    #region Public Properties

    public bool TimeMissionCompleted => _passedTime < TimeLimit;
    public bool AlarmMissionCompleted => !GameManager.Instance.IsAlarmActive;
    public bool ChipsetMissionCompleted => IsNessaryChipsetCollected();

    #endregion

    [HideInInspector] public int ChipsetCount;
    [HideInInspector, Tooltip("Seconds")] public float TimeLimit;

    [SerializeField] private Light _directionalLight;
    
    #region Private Fields

    private int _collectedChipsetCount = 0;
    private float _passedTime;
    private bool _levelStarted = false;
    private AudioSource _audioSource;

    #endregion

    #region Unity Events

    void Awake()
    {
        ChipsetCount = LevelData.ChipsetCount;
        TimeLimit = LevelData.TimeLimit;
        _audioSource = GetComponent<AudioSource>();
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

    void Update()
    {
        if(!_levelStarted) return;

        _passedTime += Time.deltaTime / TimeLimit;
    }

    #endregion

    public void CollectChipset() => _collectedChipsetCount++;

    public void PlayHackSFX()
    {
        if(Settings.Instance.Sound == 1)
        _audioSource.Play();
    }

    #region Private Methods

    private bool IsNessaryChipsetCollected() => _collectedChipsetCount >= ChipsetCount;

    private void IncreaseProtocolCount(GameState gameState)
    {
        if(gameState != GameState.Win) return;

        if(IsNessaryChipsetCollected()) GameManager.Instance.ProtocolCount++;

        if(!GameManager.Instance.IsAlarmActive) GameManager.Instance.ProtocolCount++;

        if(_passedTime < TimeLimit) GameManager.Instance.ProtocolCount++;
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

    #endregion
}