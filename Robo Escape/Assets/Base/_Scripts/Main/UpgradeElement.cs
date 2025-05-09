using UnityEngine;
using UnityEngine.UI;

public class UpgradeElement : MonoBehaviour
{
    [SerializeField] private UpgradeElementData _upgradeElementData;
    [SerializeField] private Button _increaseButton;
    [SerializeField] private TMPro.TMP_Text _priceText;
    [SerializeField] private Image _iconImage;

    void Awake()
    {
        _priceText.text = _upgradeElementData.price.ToString();
        _iconImage.sprite = _upgradeElementData.icon;
        _increaseButton.onClick.RemoveAllListeners();
        _increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
    }

    private void OnIncreaseButtonClicked()
    {
        _upgradeElementData.onBuy?.Invoke();
    }
}
