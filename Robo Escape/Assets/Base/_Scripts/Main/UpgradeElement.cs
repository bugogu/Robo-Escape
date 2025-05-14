using UnityEngine;

public class UpgradeElement : MonoBehaviour
{
    [SerializeField] private UpgradeElementData _upgradeElementData;
    [SerializeField] private TMPro.TMP_Text _priceText;
    [SerializeField] private TMPro.TMP_Text _titleText;

    void Awake()
    {
        _priceText.text = _upgradeElementData.Price.ToString();
        _titleText.text = _upgradeElementData.Title;
    }
}
