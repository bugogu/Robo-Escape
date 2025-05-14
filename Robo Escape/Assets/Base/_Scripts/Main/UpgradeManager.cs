using UnityEngine;

public class UpgradeManager : MonoSingleton<UpgradeManager>
{
    public int EnergyCapacity
    {
        get => PlayerPrefs.GetInt(Consts.Prefs.CAPACITY, 0);
        set => PlayerPrefs.SetInt(Consts.Prefs.CAPACITY, value);
    }

    private DoUpgrade _doUpgrade;

    void Awake()
    {
        _doUpgrade = GetComponent<DoUpgrade>();

        for (int i = 0; i < EnergyCapacity; i++)
            _doUpgrade.GenerateProgressEnergyCapacity();

        if(_doUpgrade._upgradeEnergyCapacity.MaxIncreaseCount == EnergyCapacity)
            Invoke(nameof(SetTextWithDelay), 0.0001f);
    }

    private void SetTextWithDelay() => _doUpgrade.SetEnergyPriceTextToMax();
}
