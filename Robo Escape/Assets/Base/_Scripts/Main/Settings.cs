using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class Settings : MonoSingleton<Settings>
{
    [SerializeField] private Button _settingsButton;
    public RectTransform _settingsPanel;
    [SerializeField] private float _openingTime;

    public event Action<bool> OnOutlinesSetted;

    public int Music 
    {
        get => PlayerPrefs.GetInt(Consts.Prefs.MUSIC, 0);
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

    public int Outlines 
    {
        get => PlayerPrefs.GetInt(Consts.Prefs.OUTLINES, 1);
        set => PlayerPrefs.SetInt(Consts.Prefs.OUTLINES, value);
    }

    private bool _isOpen;
    private AudioSource _audioSource;

    void Awake()
    {
        _settingsButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.AddListener(SettingsPanel);

        _audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        OnOutlinesSetted?.Invoke(Outlines == 1);
    }

    public void SettingsPanel()
    {
        PlayButtonSound();

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

    public void SetOutlines(bool status)
    {
        OnOutlinesSetted?.Invoke(status);   
    }

    public void PlayButtonSound() 
    {
        if(Sound == 1)
            _audioSource?.Play();
    } 
}
