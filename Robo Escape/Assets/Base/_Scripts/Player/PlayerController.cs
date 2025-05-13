using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    [SerializeField] private GameDesignData gameDesignData;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Color initialOutlineColor;
    [SerializeField] private float shieldDuration = 10f;
    [SerializeField] private GameObject playerMovingFX;
    //[SerializeField] private GameObject passwordCanvas;
    [SerializeField] private ParticleSystem antiAlarmShieldFX;
    [SerializeField] private GameObject flashTrailFX;
    [SerializeField] private GameObject magneticPulseAuraFX;
    [SerializeField] private GameObject magneticPulseFX;
    [SerializeField] private GameObject magneticPulseRadiusSprite;
    [SerializeField] private float magneticPulseRadius;
    [SerializeField] private Volume globalVolume;
    [SerializeField] private Color hitVignetteColor = Color.red;
    [Range(0f, 1f), SerializeField] private float hitEffectsRestartTime = 0.2f;

    private PlayerMovement _playerMovement;
    private bool _isFlashActive = false;
    private float _flashDuration = 10f;
    private float _initialAnimatorSpeed;

    private Color _flashOutline;
    private Color _empOutline;
    private Color _shieldOutline;
    private Color _initialOutline;
    private Vignette _vignette;
    private int _outlineColor = Shader.PropertyToID("_Color");

    void Start()
    {
        _flashDuration = gameDesignData.flashPowerUpDuration;
        _initialAnimatorSpeed = GetComponent<Animator>().speed;

        outlineMaterial.SetColor(_outlineColor, initialOutlineColor);

        if(globalVolume.profile.TryGet(out Vignette v)) _vignette = v;

        if(Settings.Instance != null && Settings.Instance.Outlines == 1)
        {
            _initialOutline = outlineMaterial.GetColor("_Color");
            _flashOutline = gameDesignData.flashOutlineColor;
            _empOutline = gameDesignData.empOutlineColor;
            _shieldOutline = gameDesignData.shieldOutlineColor;
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
        if (GameManager.Instance != null)
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
            playerMovingFX.SetActive(status);

        if(_isFlashActive)
            flashTrailFX.SetActive(status);
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

        UIManager.Instance.ActivatePowerCounter(shieldDuration, true);

        if(Settings.Instance.Outlines == 1)
            outlineMaterial.SetColor("_Color", _shieldOutline);

        _hasAnyPowerUp = true;
        _isProtectionActive = true;
        antiAlarmShieldFX.gameObject.SetActive(true);

        Invoke("RemoveProtection", shieldDuration);
    }

    private void GainFlash()
    {
        if(_hasAnyPowerUp) return;

        UIManager.Instance.ActivatePowerCounter(_flashDuration, false);

        if(Settings.Instance.Outlines == 1)
            outlineMaterial.SetColor("_Color", _flashOutline);

        GetComponent<Animator>().speed = gameDesignData.flashSpeedMultiplier;

        _playerMovement.SetSpeed(true);

        _hasAnyPowerUp = true;
        _isFlashActive = true;

        flashTrailFX.SetActive(true);
        playerMovingFX.SetActive(false);

        Invoke(nameof(RemoveFlash), _flashDuration);
    }

    public void RemoveProtection()
    {
        if(!_isProtectionActive) return;

        if(Settings.Instance.Outlines == 1)
            outlineMaterial.SetColor("_Color", _initialOutline);

        _isProtectionActive = false;
        antiAlarmShieldFX.gameObject.SetActive(false);
        _hasAnyPowerUp = false;
    }

    private void RemoveFlash()
    {
        GetComponent<Animator>().speed = _initialAnimatorSpeed;

        if(Settings.Instance.Outlines == 1)
            outlineMaterial.SetColor("_Color", _initialOutline);

        _playerMovement.SetSpeed(false);

        _hasAnyPowerUp = false;
        _isFlashActive = false;

        flashTrailFX.SetActive(false);
        playerMovingFX.SetActive(true);
    }

    public bool GetFlashStatus() => _isFlashActive;

    private void GainMagneticPulse()
    {
        if(_hasAnyPowerUp) return;

        if(Settings.Instance.Outlines == 1)
            outlineMaterial.SetColor("_Color", _empOutline);

        magneticPulseRadiusSprite.SetActive(true);
        UIManager.Instance.magneticPulseButton.transform.parent.gameObject.SetActive(true);
        _hasAnyPowerUp = true;
        magneticPulseAuraFX.SetActive(true);
        _hasMagneticCharge = true;
    }

    public void UseMagneticPulse()
    {
        Emp();

        CameraShake.Shake();
        SoundManager.Instance.PlaySFX(SoundManager.Instance.electricSfx);

        if(Settings.Instance.Outlines == 1)
            outlineMaterial.SetColor("_Color", _initialOutline);
        
        magneticPulseRadiusSprite.SetActive(false);
        UIManager.Instance.magneticPulseButton.transform.parent.gameObject.SetActive(false);
        _hasAnyPowerUp = false;
        magneticPulseAuraFX.SetActive(false);
        _hasMagneticCharge = false;
        magneticPulseFX.SetActive(true);
        Invoke(nameof(CloseMagneticPulseFX), 2f);
    }

    private void Emp()
    {
        EnergyBar.Instance.ConsumeEnergy(EnergyBar.Instance._maxEnergyCapacity/2, true);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, magneticPulseRadius);

        foreach (Collider col in hitColliders)
        {
            IEffectableFromEMP empTarget = col.GetComponent<IEffectableFromEMP>();
            
            if (empTarget != null)
                empTarget.EffectFromEMP();
        }
    }

    private void CloseMagneticPulseFX()=>
        magneticPulseFX.SetActive(false);

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, magneticPulseRadius);
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
        if(Settings.Instance.Outlines == 1) outlineMaterial.SetColor("_Color", hitVignetteColor);
        _vignette.color.Override(hitVignetteColor);
        EnergyBar.Instance.ConsumeEnergy(energyDamage, true);
        CameraShake.Shake();
        // GameManager.Instance.SetAlarm(true);
        Invoke(nameof(RestartHitEffects), hitEffectsRestartTime);
    }

    private void RestartHitEffects()
    {
        if(Settings.Instance.Outlines == 1) outlineMaterial.SetColor("_Color", initialOutlineColor);
        _vignette.color.Override(Color.black);
    }
}
