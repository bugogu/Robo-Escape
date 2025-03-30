using System;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private PlayableDirector _playableDirector;
    private GameState _currentGameState;

    public event Action<GameState> OnGameStateChanged;

    public void ChangeGameState(GameState gameState) 
    {
        OnGameStateChanged?.Invoke(gameState);
        _currentGameState = gameState;
        Debug.Log("Game State : " + gameState);
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.Intro);
        _playableDirector.stopped += OnTimeLineFinished;
    }

    private void OnTimeLineFinished(PlayableDirector director)=>
        ChangeGameState(GameState.Play);
}
