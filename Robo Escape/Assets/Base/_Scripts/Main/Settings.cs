using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Settings : MonoSingleton<Settings>
{
    [SerializeField] private Button _settingsButton;
    [SerializeField] private RectTransform _settingsPanel;
    [SerializeField] private float _openingTime;

    public int Music 
    {
        get => PlayerPrefs.GetInt(Consts.Prefs.MUSIC, 1);
        set => PlayerPrefs.SetInt(Consts.Prefs.MUSIC, value);
    } 

    public int Sound 
    {
        get => PlayerPrefs.GetInt(Consts.Prefs.SOUND, 1);
        set => PlayerPrefs.SetInt(Consts.Prefs.SOUND, value);
    }

    public int Haptic 
    {
        get => PlayerPrefs.GetInt(Consts.Prefs.HAPTIC, 1);
        set => PlayerPrefs.SetInt(Consts.Prefs.HAPTIC, value);
    }

    private bool _isOpen;

    void Awake()
    {
        _settingsButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.AddListener(SettingsPanel);
    }

    private void SettingsPanel()
    {
        if(!_isOpen)
        {
            _isOpen = true;
            _settingsButton.interactable = false;
            _settingsPanel.gameObject.SetActive(_isOpen);
            _settingsPanel.DOScale(Vector3.one, _openingTime).SetEase(Ease.OutBack).OnComplete(()=> _settingsButton.interactable = true);
        }
        else
        {
            _isOpen = false;
            _settingsButton.interactable = false;
            _settingsPanel.DOScale(Vector3.zero, _openingTime).OnComplete(()=> _settingsPanel.gameObject.SetActive(_isOpen)).OnComplete(()=> _settingsButton.interactable = true);
        }
    }
}
