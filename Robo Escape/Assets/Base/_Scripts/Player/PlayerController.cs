using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    public ParticleSystem _energyCellFX;
    public ParticleSystem _drainCellFX;
    
    public ParticleSystem _gateHackFX;
    public ParticleSystem _blueHackFX;
    public ParticleSystem _yellowHackFX;
    public ParticleSystem _purpleHackFX;

    [SerializeField] private GameObject _playerMovingFX;
    [SerializeField] private GameObject _passwordCanvas;

    private PlayerMovement _playerMovement;

    void OnEnable()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.enabled = false;
        GameManager.Instance.OnGameStateChanged += CanMove;
    }

    void OnDisable()
    {
        // Önemsiz bir null hatası veriyor.
        GameManager.Instance.OnGameStateChanged -= CanMove;
    }

    private void CanMove(GameState gameState)
    {
        if(gameState == GameState.Play) _playerMovement.enabled = true;
        else _playerMovement.enabled = false;
    }

    public void MovingFX(bool status)=>
        _playerMovingFX.SetActive(status);

    public void HackFxActive(InteractionType interactionType, bool status)
    {
        switch (interactionType)
        {
            case InteractionType.Gate:
                _gateHackFX.gameObject.SetActive(status);
                break;
            case InteractionType.YellowBox:
                _yellowHackFX.gameObject.SetActive(status);
                break;
            case InteractionType.BlueBox:
                _blueHackFX.gameObject.SetActive(status);
                break;
            case InteractionType.PurpleBox:
                _purpleHackFX.gameObject.SetActive(status);
                break;
        }
    }
}
