namespace Player
{
    using UnityEngine;
    using DG.Tweening;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;
    using UnityEngine.Serialization;
    using System.Collections;

    public class PlayerController : MonoSingleton<PlayerController>
    {
        #region Public Fields

        [HideInInspector] public bool IsProtectionActive = false;
        [HideInInspector] public bool HasAnyPowerUp = false;
        [HideInInspector] public bool HasMagneticCharge = false;
        [HideInInspector] public Coroutine CurrentShieldCoroutine;

        #endregion

        #region References

        public ParticleReferences ParticleReferences;

        [Space(20)][FormerlySerializedAs("teleportFX")] public GameObject TeleportFX;
        [FormerlySerializedAs("gameDesignData"), SerializeField] private GameDesignData _gameDesignData;
        [FormerlySerializedAs("outlineMaterial"), SerializeField] private Material _outlineMaterial;
        [FormerlySerializedAs("initialOutlineColor"), SerializeField] private Color _initialOutlineColor;
        [FormerlySerializedAs("shieldDuration"), SerializeField] private float _shieldDuration = 10f;
        [FormerlySerializedAs("playerMovingFX"), SerializeField] private GameObject _playerMovingFX;
        [FormerlySerializedAs("antiAlarmShieldFX"), SerializeField] private ParticleSystem _antiAlarmShieldFX;
        [FormerlySerializedAs("flashTrailFX"), SerializeField] private GameObject _flashTrailFX;
        [FormerlySerializedAs("magneticPulseAuraFX"), SerializeField] private GameObject _magneticPulseAuraFX;
        [FormerlySerializedAs("magneticPulseFX"), SerializeField] private GameObject _magneticPulseFX;
        [FormerlySerializedAs("magneticPulseRadiusSprite"), SerializeField] private GameObject _magneticPulseRadiusSprite;
        [FormerlySerializedAs("magneticPulseRadius"), SerializeField] private float _magneticPulseRadius;
        [FormerlySerializedAs("globalVolume"), SerializeField] private Volume _globalVolume;
        [FormerlySerializedAs("hitVignetteColor"), SerializeField] private Color _hitVignetteColor = Color.red;
        [SerializeField] private GameObject _shadow, _dieFX;
        [SerializeField] private Transform _bodyParent;
        [SerializeField] private float _dieForce = 10f;
        [SerializeField] private Unity.Cinemachine.CinemachineCamera _cinemachineCamera;
        [Range(0f, 1f), SerializeField] private float _hitEffectsRestartTime = .2f;
        
        #endregion

        #region Private Fields

        private PlayerMovement _playerMovement;
        private bool _isFlashActive = false;
        private float _flashDuration = 10f;
        private float _initialAnimatorSpeed;

        private Color _flashOutline, _empOutline, _shieldOutline, _initialOutline;
        private Vignette _vignette;
        private int _outlineColor = Shader.PropertyToID("_Color");
        private Collider _playerCollider => GetComponent<Collider>();

        #endregion

        #region Unity Events

        void Start()
        {
            _flashDuration = _gameDesignData.FlashPowerUpDuration;
            _initialAnimatorSpeed = GetComponent<Animator>().speed;

            _outlineMaterial.SetColor(_outlineColor, _initialOutlineColor);

            if (_globalVolume.profile.TryGet(out Vignette v)) _vignette = v;

            if (Settings.Instance != null && Settings.Instance.Outlines == 1)
            {
                _initialOutline = _outlineMaterial.GetColor("_Color");
                _flashOutline = _gameDesignData.FlashOutlineColor;
                _empOutline = _gameDesignData.EmpOutlineColor;
                _shieldOutline = _gameDesignData.ShieldOutlineColor;
            }
        }

        void OnEnable()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerMovement.enabled = false;
            GameManager.Instance.OnGameStateChanged += CanMove;
            GameManager.Instance.OnGameStateChanged += Die;
            UIManager.Instance.MagneticPulseButton.onClick.RemoveAllListeners();
            UIManager.Instance.MagneticPulseButton.onClick.AddListener(UseMagneticPulse);
        }

        void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged -= CanMove;
                GameManager.Instance.OnGameStateChanged -= Die;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _magneticPulseRadius);
        }

        #endregion

        #region Public Methods

        public void Jump(Transform target, float jumpPower, float jumpDuration)
        {
            Vector3 dir = transform.position - target.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-dir), 1);

            //CameraShake.Shake();
            _playerMovement.OnGround = false;
            transform.DOJump(target.position, jumpPower, 1, jumpDuration).SetUpdate(UpdateType.Fixed).
            OnComplete(() => _playerMovement.OnGround = true).SetEase(Ease.Linear);
        }

        public void GetHit(float energyDamage)
        {
            if (Settings.Instance.Outlines == 1) _outlineMaterial.SetColor("_Color", _hitVignetteColor);
            _vignette.color.Override(_hitVignetteColor);
            EnergyBar.Instance.ConsumeEnergy(energyDamage, true);
            CameraShake.Shake();
            ParticleReferences.DrainCellFX.Play();
            // GameManager.Instance.SetAlarm(true);
            Invoke(nameof(RestartHitEffects), _hitEffectsRestartTime);
        }

        public void Die(GameState state)
        {
            if (GameManager.Instance.GetCurrentState() != GameState.Lose) return;

            _cinemachineCamera.Follow = null;
            GetComponent<Animator>().enabled = false;
            _playerCollider.enabled = false;
            _shadow.SetActive(false);
            _dieFX.SetActive(true); _dieFX.transform.parent = null;
            
            GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;

            GetComponent<Rigidbody>().AddForce(transform.up * _dieForce);
        }

        public void MovingFX(bool status)
        {
            if (!_isFlashActive)
                _playerMovingFX.SetActive(status);

            if (_isFlashActive)
                _flashTrailFX.SetActive(status);
        }

        public void HackFxActive(InteractionType interactionType, bool status)
        {
            switch (interactionType)
            {
                case InteractionType.Red:
                    ParticleReferences.RedHackFX.gameObject.SetActive(status);
                    break;
                case InteractionType.YellowBox:
                    ParticleReferences.YellowHackFX.gameObject.SetActive(status);
                    break;
                case InteractionType.BlueBox:
                    ParticleReferences.BlueHackFX.gameObject.SetActive(status);
                    break;
                case InteractionType.PurpleBox:
                    ParticleReferences.PurpleHackFX.gameObject.SetActive(status);
                    break;
            }
        }

        public void GainPowerUp(PowerUpType powerUpType)
        {
            switch (powerUpType)
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

        public void UseMagneticPulse()
        {
            Emp();

            CameraShake.Shake();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.ElectricSfx);

            if (Settings.Instance.Outlines == 1)
                _outlineMaterial.SetColor("_Color", _initialOutline);

            _magneticPulseRadiusSprite.SetActive(false);
            UIManager.Instance.MagneticPulseButton.transform.parent.gameObject.SetActive(false);
            HasAnyPowerUp = false;
            _magneticPulseAuraFX.SetActive(false);
            HasMagneticCharge = false;
            _magneticPulseFX.SetActive(true);
            Invoke(nameof(CloseMagneticPulseFX), 2f);
        }

        public bool GetFlashStatus() => _isFlashActive;

        public void RemoveProtection()
        {
            if (!IsProtectionActive) return;

            if (Settings.Instance.Outlines == 1)
                _outlineMaterial.SetColor("_Color", _initialOutline);

            IsProtectionActive = false;
            _antiAlarmShieldFX.gameObject.SetActive(false);
            HasAnyPowerUp = false;
            UIManager.Instance.DeactivatePowerCounter();
            SoundManager.Instance.PlaySFX(SoundManager.Instance.PropBreakSfx);

            if(UIManager.Instance.PowerUpTween != null)
                UIManager.Instance.PowerUpTween.Kill();
            
        }

        #endregion

        #region Private Methods

        private void GainAntiAlarmShield()
        {
            if (IsProtectionActive) return;

            UIManager.Instance.ActivatePowerCounter(_shieldDuration, true);

            if (Settings.Instance.Outlines == 1)
                _outlineMaterial.SetColor("_Color", _shieldOutline);

            HasAnyPowerUp = true;
            IsProtectionActive = true;
            _antiAlarmShieldFX.gameObject.SetActive(true);
        }

        private void GainFlash()
        {
            if (HasAnyPowerUp) return;

            UIManager.Instance.ActivatePowerCounter(_flashDuration, false);

            if (Settings.Instance.Outlines == 1)
                _outlineMaterial.SetColor("_Color", _flashOutline);

            GetComponent<Animator>().speed = _gameDesignData.FlashSpeedMultiplier;

            _playerMovement.SetSpeed(true);

            HasAnyPowerUp = true;
            _isFlashActive = true;

            _flashTrailFX.SetActive(true);
            _playerMovingFX.SetActive(false);

            Invoke(nameof(RemoveFlash), _flashDuration);
        }

        private void RemoveFlash()
        {
            GetComponent<Animator>().speed = _initialAnimatorSpeed;

            if (Settings.Instance.Outlines == 1)
                _outlineMaterial.SetColor("_Color", _initialOutline);

            _playerMovement.SetSpeed(false);

            HasAnyPowerUp = false;
            _isFlashActive = false;

            _flashTrailFX.SetActive(false);
            _playerMovingFX.SetActive(true);
        }

        private void GainMagneticPulse()
        {
            if (HasAnyPowerUp) return;

            if (Settings.Instance.Outlines == 1)
                _outlineMaterial.SetColor("_Color", _empOutline);

            _magneticPulseRadiusSprite.SetActive(true);
            UIManager.Instance.MagneticPulseButton.transform.parent.gameObject.SetActive(true);
            HasAnyPowerUp = true;
            _magneticPulseAuraFX.SetActive(true);
            HasMagneticCharge = true;
        }

        private void Emp()
        {
            EnergyBar.Instance.ConsumeEnergy(EnergyBar.Instance.MaxEnergyCapacity / 2, true);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _magneticPulseRadius);

            foreach (Collider col in hitColliders)
            {
                IEffectableFromEMP empTarget = col.GetComponent<IEffectableFromEMP>();

                if (empTarget != null)
                    empTarget.EffectFromEMP();
            }
        }

        private void CloseMagneticPulseFX() =>
            _magneticPulseFX.SetActive(false);

        private void RestartHitEffects()
        {
            if (Settings.Instance.Outlines == 1) _outlineMaterial.SetColor("_Color", _initialOutlineColor);
            _vignette.color.Override(Color.black);
        }

        private void CanMove(GameState gameState)
        {
            if (gameState == GameState.Play) _playerMovement.enabled = true;
            else _playerMovement.enabled = false;
        }

        #endregion
    }

    [System.Serializable]
    public class ParticleReferences
    {
        public ParticleSystem EnergyCellFX;
        public ParticleSystem DrainCellFX;
        public ParticleSystem RedHackFX;
        public ParticleSystem BlueHackFX;
        public ParticleSystem YellowHackFX;
        public ParticleSystem PurpleHackFX;
    }
}