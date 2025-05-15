using System;
using UnityEngine;
using UnityEngine.Playables;
using Player;

public class GameManager : MonoSingleton<GameManager>
{
    #region Public Fields
    
    public event Action<GameState> OnGameStateChanged;
    public event Action<bool> OnAlarmSetted;

    [HideInInspector] public bool WaterLevel;
    [HideInInspector] public bool IsAlarmActive = false;

    #endregion

    #region References

    [SerializeField] private PlayableDirector _playableDirector;
    [SerializeField] private Material[] _outlines;
    [SerializeField] private float[] _initialScaleValues;

    #endregion

    public int ProtocolCount 
    { 
        get => PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0); 
        set => PlayerPrefs.SetInt(Consts.Prefs.PROTOCOLCOUNT, value);
    }

    private GameState _gameState;

    #region Unity Events

    void Start()
    {
        WaterLevel = LevelManager.Instance.LevelData.WaterLevel;

        Settings.Instance?.SetOutlines(Settings.Instance.Outlines == 1);
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.Intro);
        _playableDirector.stopped += OnTimeLineFinished;
        OnAlarmSetted += PlayAlarmSound;
        OnAlarmSetted += DoShake;
        OnGameStateChanged += StopAlarmSound;

        if (Settings.Instance != null)
            Settings.Instance.OnOutlinesSetted += SetBasicOutlines;
    }

    void OnDisable()
    {
        _playableDirector.stopped -= OnTimeLineFinished;
        OnAlarmSetted -= PlayAlarmSound;
        OnAlarmSetted -= DoShake;
        OnGameStateChanged -= StopAlarmSound;

        if (Settings.Instance != null)
            Settings.Instance.OnOutlinesSetted -= SetBasicOutlines;
    }

    #endregion

    #region Public Methods

    public void ChangeGameState(GameState gameState) 
    {
        _gameState = gameState;
        OnGameStateChanged?.Invoke(gameState);
        Debug.Log("Game State : " + gameState);
    }

    public void SetAlarm(bool status)
    {
        if(PlayerController.Instance.IsProtectionActive)
        {
            PlayerController.Instance.RemoveProtection();
            return;
        }

        OnAlarmSetted?.Invoke(status);
        IsAlarmActive = status;
    }

    public void SetAlarm(bool status, bool passwordTriggered = false)
    {
        if(!passwordTriggered) return;
        
        OnAlarmSetted?.Invoke(status);
        IsAlarmActive = status;
    }

    public GameState GetCurrentState() 
    {
        return _gameState;
    } 

    #endregion
    
    #region Private Methods

    private void OnTimeLineFinished(PlayableDirector director)=>
        ChangeGameState(GameState.Play);

    private void PlayAlarmSound(bool alarmActive)
    {
        if(Settings.Instance.Sound == 1) GetComponent<AudioSource>()?.Play();
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Stop();
    }

    private void StopAlarmSound(GameState gameState)
    {
        if(gameState == GameState.Win)
        GetComponent<AudioSource>()?.Stop();  
    }

    private void SetBasicOutlines(bool status) 
    {
        if(status) 
            for (int i = 0; i < _outlines.Length; i++) _outlines[i].SetFloat("_Scale", _initialScaleValues[i]);

        if(!status) 
            for (int i = 0; i < _outlines.Length; i++) _outlines[i].SetFloat("_Scale", 1);
    }

    private void DoShake(bool alarmActive)
    {
        if(alarmActive)
        CameraShake.Shake();
    }

    #endregion
}