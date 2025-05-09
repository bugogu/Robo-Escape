using UnityEngine;
using UnityEngine.UI;


public class EnergyBar : MonoSingleton<EnergyBar>
{
    [HideInInspector] public float _maxEnergyCapacity;

    [SerializeField] private GameDesignData _gameDesignData;
    [SerializeField] private Image _energyBarFillImage;
    [SerializeField] private TMPro.TMP_Text _energyText;

    void Awake()
    {
        _maxEnergyCapacity = _gameDesignData.maxEnergyCapacity + (PlayerPrefs.GetInt(Consts.Prefs.CAPACITY, 0) * _gameDesignData.energyCapacityUpgradeAmount);
    }

    public void ConsumeEnergy(float amount, bool burst = false)
    {
        if (amount <= 0) return;

        if(burst)
        EnergyPopUpText.Instance.ShowEnergyPopup((int)amount, false);

        float newEnergy = GetCurrentEnergy() - amount;
        SetEnergy(newEnergy);

        EnoughForMagneticPulse();
    }

    public void ReplenishEnergy(float amount, bool burst = false)
    {
        if (amount <= 0) return;

        if(burst)
        EnergyPopUpText.Instance.ShowEnergyPopup((int)amount, true);

        float newEnergy = GetCurrentEnergy() + amount;
        SetEnergy(newEnergy);

        EnoughForMagneticPulse();
    }

    public float GetCurrentEnergy()
    {
        return _energyBarFillImage.fillAmount * _maxEnergyCapacity;
    }

    private void SetEnergy(float newEnergy)
    {
        float clampedEnergy = Mathf.Clamp(newEnergy, 0f, _maxEnergyCapacity);
        UpdateEnergyUI(clampedEnergy);
    }

    private void UpdateEnergyUI(float energyAmount)
    {
        _energyBarFillImage.fillAmount = energyAmount / _maxEnergyCapacity;
        _energyText.text = energyAmount.ToString("0") + "/" + _maxEnergyCapacity.ToString();
    }

    private void EnoughForMagneticPulse() 
    {
        if(!PlayerController.Instance._hasMagneticCharge) return;

        if(GetCurrentEnergy() >= _maxEnergyCapacity/2)
        UIManager.Instance.magneticPulseButton.interactable = true;
        else
        UIManager.Instance.magneticPulseButton.interactable = false;
    }
}
