using UnityEngine;

public class EnergyBar : MonoSingleton<EnergyBar>
{
    [HideInInspector] public float MaxEnergyCapacity;

    #region References

    [SerializeField] private GameDesignData _gameDesignData;
    [SerializeField] private UnityEngine.UI.Image _energyBarFillImage;
    [SerializeField] private TMPro.TMP_Text _energyText;

    #endregion

    void Awake() =>
        MaxEnergyCapacity = _gameDesignData.MaxEnergyCapacity + (PlayerPrefs.GetInt(Consts.Prefs.CAPACITY, 0)
        * _gameDesignData.EnergyCapacityUpgradeAmount);

    #region Public Methods

    /// <param name="burst"> If true, the amount will be shown in the UI</param>
    public void ConsumeEnergy(float amount, bool burst = false)
    {
        if (amount <= 0) return;

        var newEnergy = GetCurrentEnergy() - amount;

        SetEnergy(newEnergy);
        EnoughForMagneticPulse();

        if (burst)
        {
            EnergyPopUpText.Instance.ShowEnergyPopup((int)amount, false);
            ReduceMaxEnergy(amount);

            if (MaxEnergyCapacity <= 0)
                Invoke(nameof(ZeroEnergyAction), 0.1f);
        }
    }

    /// <param name="burst"> If true, the amount will be shown in the UI</param>
    public void ReplenishEnergy(float amount, bool burst = false)
    {
        if (amount <= 0) return;

        if (burst)
            EnergyPopUpText.Instance.ShowEnergyPopup((int)amount, true);

        var newEnergy = GetCurrentEnergy() + amount;
        SetEnergy(newEnergy);

        EnoughForMagneticPulse();
    }

    public float GetCurrentEnergy()
    {
        return _energyBarFillImage.fillAmount * MaxEnergyCapacity;
    }

    #endregion

    #region Private Methods

    private void SetEnergy(float newEnergy)
    {
        var clampedEnergy = Mathf.Clamp(newEnergy, 0f, MaxEnergyCapacity);
        UpdateEnergyUI(clampedEnergy);
    }

    private void ReduceMaxEnergy(float amount) =>
        MaxEnergyCapacity -= amount;

    private void UpdateEnergyUI(float energyAmount)
    {
        _energyBarFillImage.fillAmount = energyAmount / MaxEnergyCapacity;
        _energyText.text = energyAmount.ToString("0") + "/" + MaxEnergyCapacity.ToString();
    }

    private void EnoughForMagneticPulse()
    {
        if (!Player.PlayerController.Instance.HasMagneticCharge) return;

        if (GetCurrentEnergy() >= MaxEnergyCapacity / 2)
            UIManager.Instance.MagneticPulseButton.interactable = true;
        else
            UIManager.Instance.MagneticPulseButton.interactable = false;
    }

    private void ZeroEnergyAction() => GameManager.Instance.ChangeGameState(GameState.Lose);
    #endregion
}