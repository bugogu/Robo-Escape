using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class EnergyPopUpText : MonoSingleton<EnergyPopUpText>
{
    [Header("References")]
    [FormerlySerializedAs("popupText"), SerializeField] private TMPro.TextMeshProUGUI _popupText;
    [FormerlySerializedAs("textRectTransform"), SerializeField] private RectTransform _textRectTransform;
    [SerializeField] private GameDesignData _gameDesignData;

    #region Private Fields

    private float _moveDistance = 100f, _moveDuration = .8f, _returnDuration = .3f, _displayDuration = 1f;
    private Color _positiveColor, _negativeColor;
    private Vector2 _originalPosition;
    private bool _isAvailable = true;

    #endregion

    private void Awake()
    {
        _originalPosition = _textRectTransform.anchoredPosition;
        _popupText.text = "";

        _moveDistance = _gameDesignData.MoveDistance;
        _moveDuration = _gameDesignData.MoveDuration;
        _returnDuration = _gameDesignData.ReturnDuration;
        _displayDuration = _gameDesignData.DisplayDuration;
        _positiveColor = _gameDesignData.PositiveColor;
        _negativeColor = _gameDesignData.NegativeColor;
    }

    public void ShowEnergyPopup(int energyAmount, bool isPositive)
    {
        if (!_isAvailable) return;

        _isAvailable = false;

        SetPopUpText(energyAmount, isPositive);

        HandlePopUpTextAnimation();
    }

    private void HandlePopUpTextAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(_textRectTransform.DOAnchorPosY(_originalPosition.y + _moveDistance, _moveDuration)
            .SetEase(Ease.OutQuad));

        sequence.AppendInterval(_displayDuration);

        sequence.Append(_textRectTransform.DOAnchorPosY(_originalPosition.y, _returnDuration)
            .SetEase(Ease.InQuad));

        sequence.OnComplete(() =>
        {
            _popupText.text = "";
            _isAvailable = true;
        });
    }

    private void SetPopUpText(int value, bool mark)
    { 
        if (mark)
        {
            _popupText.text = $"+{value}";
            _popupText.color = _positiveColor;
        }
        else
        {
            _popupText.text = $"-{value}";
            _popupText.color = _negativeColor;
        }
    }
}