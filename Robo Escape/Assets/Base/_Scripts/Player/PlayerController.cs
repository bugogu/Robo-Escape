using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class PlayerController : MonoSingleton<PlayerController>
{
    [HideInInspector] public bool _isProtectionActive = false;
    [HideInInspector] public bool _hasAnyPowerUp = false;
    [HideInInspector] public bool _hasMagneticCharge = false;

    public ParticleSystem _energyCellFX;
    public ParticleSystem _drainCellFX;
    
    public ParticleSystem _redHackFX;
    public ParticleSystem _blueHackFX;
    public ParticleSystem _yellowHackFX;
    public ParticleSystem _purpleHackFX;
    public GameObject teleportFX;

    [SerializeField] private GameDesignData _gameDesignData;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private Color _initialOutlineColor;
    [SerializeField] private float _shieldDuration = 10f;
    [SerializeField] private GameObject _playerMovingFX;
    [SerializeField] private GameObject _passwordCanvas;
    [SerializeField] private ParticleSystem _antiAlarmShieldFX;
    [SerializeField] private GameObject _flashTrailFX;
    [SerializeField] private GameObject _magneticPulseAuraFX;
    [SerializeField] private GameObject _magneticPulseFX;
    [SerializeField] private GameObject _magneticPulseRadiusSprite;
    [SerializeField] private float _magneticPulseRadius;
    [SerializeField] private Volume _globalVolume;
    [SerializeField] private Color _hitVignetteColor = Color.red;
    [SerializeField] private float _hitEffectsRestartTime = 0.2f;

    private PlayerMovement _playerMovement;
    private bool _isFlashActive = false;
    private float _flashDuration = 10f;
    private float _initialAnimatorSpeed;

    private Color _flashOutline;
    private Color _empOutline;
    private Color _shieldOutline;
    private Color _initialOutline;
    private Vignette _vignette;

    void Start()
    {
        _flashDuration = _gameDesignData.flashPowerUpDuration;
        _initialAnimatorSpeed = GetComponent<Animator>().speed;

        _outlineMaterial.SetColor("_Color", _initialOutlineColor);

        if(_globalVolume.profile.TryGet(out Vignette v)) _vignette = v;

        if(Settings.Instance.Outlines == 1)
        {
            _initialOutline = _outlineMaterial.GetColor("_Color");
            _flashOutline = _gameDesignData.flashOutlineColor;
            _empOutline = _gameDesignData.empOutlineColor;
            _shieldOutline = _gameDesignData.shieldOutlineColor;
        }
    }

    void OnEnable()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerMovement.enabled = false;
        GameManager.Instance.OnGameStateChanged += CanMove;
        UIManager.Instance.magneticPulseButton.onClick.RemoveAllListeners();
        UIManager.Instance.magneticPulseButton.onClick.AddListener(UseMagneticPulse);
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
            case PowerUpType.MagneticPulse:
                GainMagneticPulse();
                break;
        }
    }
    
    private void GainAntiAlarmShield()
    {
        if(_isProtectionActive) return;

        UIManager.Instance.ActivatePowerCounter(_shieldDuration, true);

        if(Settings.Instance.Outlines == 1)
        _outlineMaterial.SetColor("_Color", _shieldOutline);

        _hasAnyPowerUp = true;
        _isProtectionActive = true;
        _antiAlarmShieldFX.gameObject.SetActive(true);

        Invoke("RemoveProtection", _shieldDuration);
    }

    private void GainFlash()
    {
        if(_hasAnyPowerUp) return;

        UIManager.Instance.ActivatePowerCounter(_flashDuration, false);

        if(Settings.Instance.Outlines == 1)
        _outlineMaterial.SetColor("_Color", _flashOutline);

        GetComponent<Animator>().speed = _gameDesignData.flashSpeedMultiplier;

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

        if(Settings.Instance.Outlines == 1)
        _outlineMaterial.SetColor("_Color", _initialOutline);

        _isProtectionActive = false;
        _antiAlarmShieldFX.gameObject.SetActive(false);
        _hasAnyPowerUp = false;
    }

    private void RemoveFlash()
    {
        GetComponent<Animator>().speed = _initialAnimatorSpeed;

        if(Settings.Instance.Outlines == 1)
        _outlineMaterial.SetColor("_Color", _initialOutline);

        _playerMovement.SetSpeed(false);

        _hasAnyPowerUp = false;
        _isFlashActive = false;

        _flashTrailFX.SetActive(false);
        _playerMovingFX.SetActive(true);
    }

    public bool GetFlashStatus() => _isFlashActive;

    private void GainMagneticPulse()
    {
        if(_hasAnyPowerUp) return;

        if(Settings.Instance.Outlines == 1)
        _outlineMaterial.SetColor("_Color", _empOutline);

        _magneticPulseRadiusSprite.SetActive(true);
        UIManager.Instance.magneticPulseButton.transform.parent.gameObject.SetActive(true);
        _hasAnyPowerUp = true;
        _magneticPulseAuraFX.SetActive(true);
        _hasMagneticCharge = true;
    }

    public void UseMagneticPulse()
    {
        EMP();

        CameraShake.Shake();

        if(Settings.Instance.Outlines == 1)
        _outlineMaterial.SetColor("_Color", _initialOutline);
        
        _magneticPulseRadiusSprite.SetActive(false);
        UIManager.Instance.magneticPulseButton.transform.parent.gameObject.SetActive(false);
        _hasAnyPowerUp = false;
        _magneticPulseAuraFX.SetActive(false);
        _hasMagneticCharge = false;
        _magneticPulseFX.SetActive(true);
        Invoke(nameof(CloseMagneticPulseFX), 2f);
        // TODO: EMP sesi oynatılabilir.
    }

    private void EMP()
    {
        EnergyBar.Instance.ConsumeEnergy(EnergyBar.Instance._maxEnergyCapacity/2, true);
        Collider[] _hitColliders = Physics.OverlapSphere(transform.position, _magneticPulseRadius);

        foreach (Collider col in _hitColliders)
        {
            IEffectableFromEMP empTarget = col.GetComponent<IEffectableFromEMP>();
            
            if (empTarget != null)
                empTarget.EffectFromEMP();
        }
    }

    private void CloseMagneticPulseFX()=>
        _magneticPulseFX.SetActive(false);

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _magneticPulseRadius);
    }

    public void Jump(Transform target, float jumpPower, float jumpDuration)
    {
        Vector3 dir = transform.position - target.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-dir), 1);

        CameraShake.Shake();
        _playerMovement.onGround = false;
        transform.DOJump(target.position, jumpPower, 1, jumpDuration).SetUpdate(UpdateType.Fixed).OnComplete(() => _playerMovement.onGround = true);
    } 

    public void GetHit(float energyDamage)
    {
        if(Settings.Instance.Outlines == 1) _outlineMaterial.SetColor("_Color", _hitVignetteColor);
        _vignette.color.Override(_hitVignetteColor);
        EnergyBar.Instance.ConsumeEnergy(energyDamage, true);
        CameraShake.Shake();
        // GameManager.Instance.SetAlarm(true);
        Invoke(nameof(RestartHitEffects), _hitEffectsRestartTime);
    }

    private void RestartHitEffects()
    {
        if(Settings.Instance.Outlines == 1) _outlineMaterial.SetColor("_Color", _initialOutlineColor);
        _vignette.color.Override(Color.black);
    }
}
