using System;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private PlayableDirector _playableDirector;
 
    public event Action<GameState> OnGameStateChanged;
    public event Action<bool> OnAlarmSetted;

    [HideInInspector]
    public bool isAlarmActive = false;

    public void ChangeGameState(GameState gameState) 
    {
        OnGameStateChanged?.Invoke(gameState);
        Debug.Log("Game State : " + gameState);
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.Intro);
        _playableDirector.stopped += OnTimeLineFinished;
    }

    void OnDisable()
    {
        _playableDirector.stopped -= OnTimeLineFinished;
    }

    private void OnTimeLineFinished(PlayableDirector director)=>
        ChangeGameState(GameState.Play);

    public void SetAlarm(bool status)
    {
        OnAlarmSetted?.Invoke(status);
        isAlarmActive = status;
    }
    
}
