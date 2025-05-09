using UnityEngine;

public class DoUpgrade : MonoBehaviour
{
    [SerializeField] private UpgradeElementData _upgradeEnergyCapacity;

    void OnEnable()
    {
        _upgradeEnergyCapacity.onBuy.AddListener(UpgradeEnergyCapacity);
    }

    public void UpgradeEnergyCapacity()
    {
        Settings.Instance.PlayButtonSound();
        Debug.Log("Upgrade Energy Capacity");
    }
}
