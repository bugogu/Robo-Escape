using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnergyPopUpText : MonoSingleton<EnergyPopUpText>
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private RectTransform textRectTransform;
    [SerializeField] private GameDesignData _gameDesignData;

    private float moveDistance = 100f;
    private float moveDuration = 0.8f;
    private float returnDuration = 0.3f;
    private float displayDuration = 1f;
    private Color _positiveColor, _negativeColor;

    private Vector2 originalPosition;
    private bool isAvailable = true;

    private void Awake()
    {
        originalPosition = textRectTransform.anchoredPosition;
        popupText.text = ""; 

        moveDistance = _gameDesignData.moveDistance;
        moveDuration = _gameDesignData.moveDuration;
        returnDuration = _gameDesignData.returnDuration;
        displayDuration = _gameDesignData.displayDuration;
        _positiveColor = _gameDesignData._positiveColor;
        _negativeColor = _gameDesignData._negativeColor;
    }

    public void ShowEnergyPopup(int energyAmount, bool positive)
    {
        if (!isAvailable) return;

        isAvailable = false;
        
        if(positive)
        {
            popupText.text = $"+{energyAmount}";
            popupText.color = _positiveColor;
        }
        else
        {
            popupText.text = $"-{energyAmount}";
            popupText.color = _negativeColor;
        }
        
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(textRectTransform.DOAnchorPosY(originalPosition.y + moveDistance, moveDuration)
            .SetEase(Ease.OutQuad));
        
        sequence.AppendInterval(displayDuration);
        
        sequence.Append(textRectTransform.DOAnchorPosY(originalPosition.y, returnDuration)
            .SetEase(Ease.InQuad));
        
        sequence.OnComplete(() => {
            popupText.text = "";
            isAvailable = true;
        });
    }
}
