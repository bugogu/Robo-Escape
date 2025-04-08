using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoSingleton<EnergyBar>
{
    [SerializeField] private float _maxEnergyCapacity = 100f;
    [SerializeField] private Image _energyBarFillImage;

    public void ConsumeEnergy(float amount, bool burst = false)
    {
        if (amount <= 0) return;

        if(burst)
        EnergyPopUpText.Instance.ShowEnergyPopup((int)amount, false);

        float newEnergy = GetCurrentEnergy() - amount;
        SetEnergy(newEnergy);
    }

    public void ReplenishEnergy(float amount, bool burst = false)
    {
        if (amount <= 0) return;

        if(burst)
        EnergyPopUpText.Instance.ShowEnergyPopup((int)amount, true);

        float newEnergy = GetCurrentEnergy() + amount;
        SetEnergy(newEnergy);
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
    }
}
