using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    [HideInInspector] public bool _isProtectionActive = false;
    [HideInInspector] public bool _hasAnyPowerUp = false;
    public ParticleSystem _energyCellFX;
    public ParticleSystem _drainCellFX;
    
    public ParticleSystem _redHackFX;
    public ParticleSystem _blueHackFX;
    public ParticleSystem _yellowHackFX;
    public ParticleSystem _purpleHackFX;

    [SerializeField] private GameDesignData _gameDesignData;
    [SerializeField] private float _shieldDuration = 10f;
    [SerializeField] private GameObject _playerMovingFX;
    [SerializeField] private GameObject _passwordCanvas;
    [SerializeField] private ParticleSystem _antiAlarmShieldFX;
    [SerializeField] private GameObject _flashTrailFX;

    private PlayerMovement _playerMovement;
    private bool _isFlashActive = false;
    private float _flashDuration = 10f;

    void Start()
    {
        _flashDuration = _gameDesignData.flashPowerUpDuration;
    }

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

    public void MovingFX(bool status)
    {
        if(!_isFlashActive)
        _playerMovingFX.SetActive(status);

        if(_isFlashActive)
        _flashTrailFX.SetActive(status);
    }
        

    public void HackFxActive(InteractionType interactionType, bool status)
    {
        switch (interactionType)
        {
            case InteractionType.Red:
                _redHackFX.gameObject.SetActive(status);
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

    public void GainPowerUp(PowerUpType powerUpType)
    {
        switch(powerUpType)
        {
            case PowerUpType.Shield:
                GainAntiAlarmShield();
                break;
            case PowerUpType.Flash:
                GainFlash();
                break;
        }
    }
    
    private void GainAntiAlarmShield()
    {
        if(_isProtectionActive) return;

        _hasAnyPowerUp = true;
        _isProtectionActive = true;
        _antiAlarmShieldFX.gameObject.SetActive(true);

        Invoke("RemoveProtection", _shieldDuration);
    }

    private void GainFlash()
    {
        if(_hasAnyPowerUp) return;

        _playerMovement.SetSpeed(true);

        _hasAnyPowerUp = true;
        _isFlashActive = true;

        _flashTrailFX.SetActive(true);
        _playerMovingFX.SetActive(false);

        Invoke(nameof(RemoveFlash), _flashDuration);
    }

    public void RemoveProtection()
    {
        if(!_isProtectionActive) return;

        _isProtectionActive = false;
        _antiAlarmShieldFX.gameObject.SetActive(false);
        _hasAnyPowerUp = false;
    }

    private void RemoveFlash()
    {
        _playerMovement.SetSpeed(false);

        _hasAnyPowerUp = false;
        _isFlashActive = false;

        _flashTrailFX.SetActive(false);
        _playerMovingFX.SetActive(true);
    }

    public bool GetFlashStatus() => _isFlashActive;
}
