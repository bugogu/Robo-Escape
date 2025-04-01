using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField] private GameObject _playerMovingFX;
    private PlayerMovement _playerMovement;

    void OnEnable()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.enabled = false;
        GameManager.Instance.OnGameStateChanged += CanMove;
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= CanMove;
    }

    private void CanMove(GameState gameState)
    {
        if(gameState == GameState.Play) _playerMovement.enabled = true;
        else _playerMovement.enabled = false;
    }

    public void MovingFX(bool status)=>
        _playerMovingFX.SetActive(status);
}
