using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private PlayableDirector _playableDirector;

    [SerializeField] private Material[] _outlines;
    [SerializeField] private float[] _initialScaleValues;
 
    [HideInInspector] public bool waterLevel;
    public event Action<GameState> OnGameStateChanged;
    public event Action<bool> OnAlarmSetted;

    [HideInInspector]
    public bool isAlarmActive = false;

    private GameState _gameState;

    public int ProtocolCount 
    { 
        get => PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0); 
        set => PlayerPrefs.SetInt(Consts.Prefs.PROTOCOLCOUNT, value);
    }

    void Start()
    {
        waterLevel = LevelManager.Instance.levelData.waterLevel;

        Settings.Instance.SetOutlines(Settings.Instance.Outlines == 1);
    }

    public void ChangeGameState(GameState gameState) 
    {
        _gameState = gameState;
        OnGameStateChanged?.Invoke(gameState);
        Debug.Log("Game State : " + gameState);
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.Intro);
        _playableDirector.stopped += OnTimeLineFinished;
        OnAlarmSetted += PlayAlarmSound;
        OnAlarmSetted += DoShake;
        OnGameStateChanged += StopAlarmSound;
        Settings.Instance.OnOutlinesSetted += SetBasicOutlines;
    }

    void OnDisable()
    {
        _playableDirector.stopped -= OnTimeLineFinished;
        OnAlarmSetted -= PlayAlarmSound;
        OnAlarmSetted -= DoShake;
        OnGameStateChanged -= StopAlarmSound;
        Settings.Instance.OnOutlinesSetted -= SetBasicOutlines;
    }

    private void OnTimeLineFinished(PlayableDirector director)=>
        ChangeGameState(GameState.Play);

    public void SetAlarm(bool status)
    {
        if(PlayerController.Instance._isProtectionActive)
        {
            PlayerController.Instance.RemoveProtection();
            return;
        }

        OnAlarmSetted?.Invoke(status);
        isAlarmActive = status;
    }

    public void SetAlarm(bool status, bool passwordTriggered = false)
    {
        if(!passwordTriggered) return;
        
        OnAlarmSetted?.Invoke(status);
        isAlarmActive = status;
    }

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

    public GameState GetCurrentState() 
    {
        return _gameState;
    } 
}
