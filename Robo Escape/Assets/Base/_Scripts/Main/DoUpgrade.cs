using UnityEngine;

public class DoUpgrade : MonoBehaviour
{
    #region References

    [Header("Energy Capacity")]
    public UpgradeElementData _upgradeEnergyCapacity;
    [SerializeField] private RectTransform _progressEnergyParent;
    [SerializeField] private GameObject _progressEnergyCapacityPrefab;
    [SerializeField] private TMPro.TMP_Text _energyCapacityPriceText;

    #endregion
    
    private UpgradeManager _upgradeManager;

    void Awake()
    {
        _upgradeManager = GetComponent<UpgradeManager>();
    }

    public void UpgradeEnergyCapacity()
    {
        if (_upgradeManager.EnergyCapacity >= _upgradeEnergyCapacity.MaxIncreaseCount) return;

        if(PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0) < _upgradeEnergyCapacity.Price) return;

        PlayerPrefs.SetInt(Consts.Prefs.PROTOCOLCOUNT, PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0) - _upgradeEnergyCapacity.Price);

        Settings.Instance.PlayButtonSound();

        MenuUI.Instance.SetProtocolText();

        _upgradeManager.EnergyCapacity++;

        if(_upgradeManager.EnergyCapacity == _upgradeEnergyCapacity.MaxIncreaseCount) 
            SetEnergyPriceTextToMax();

        GenerateProgressEnergyCapacity();
    }

    public void GenerateProgressEnergyCapacity() => Instantiate(_progressEnergyCapacityPrefab, _progressEnergyParent);
    public void SetEnergyPriceTextToMax()
    {
        _energyCapacityPriceText.text = "MAX";
        _energyCapacityPriceText.fontSize = 20;
    }
}