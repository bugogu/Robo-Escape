using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoSingleton<EnergyBar>
{
    [SerializeField] private float _maxEnergyCapacity = 100f;
    [SerializeField] private Image _energyBarFillImage;

    public void ConsumeEnergy(float amount)
    {
        if (amount <= 0) return;

        float newEnergy = GetCurrentEnergy() - amount;
        SetEnergy(newEnergy);
    }

    public void ReplenishEnergy(float amount)
    {
        if (amount <= 0) return;

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
        Debug.Log("Energy: " + clampedEnergy);
    }

    private void UpdateEnergyUI(float energyAmount)
    {
        _energyBarFillImage.fillAmount = energyAmount / _maxEnergyCapacity;
    }
}
