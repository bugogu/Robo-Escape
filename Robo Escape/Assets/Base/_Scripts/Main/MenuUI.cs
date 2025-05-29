using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuUI : MonoSingleton<MenuUI>
{
    #region References

    [SerializeField] private GameDesignData _gameDesignData;

    [Header("Texts")]
    [SerializeField] private TMPro.TMP_Text _levelText;
    [SerializeField] private TMPro.TMP_Text _levelTextBlack;
    [SerializeField] private TMPro.TMP_Text _protocolText, _capacityText;

    [Header("GameObjects")]
    [SerializeField] private GameObject _escapeButton;
    [SerializeField] private GameObject _upgradesCanvas, _settingsPanel;

    [Header("Transforms")]
    [SerializeField] private RectTransform _capacityRect;
    [SerializeField] private RectTransform _handRect;
    
    [Header("Floats")]
    [SerializeField] private float _showDuration = 1f;
    [SerializeField] private float _closeDuration = 3f;
    [SerializeField] private float _handRectMoveDuration = 1f;
    
    [Header("Others")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Settings _settings;
    [SerializeField] private Vector3 _handRectYThreshold = new Vector3(-140, 160, 0);
    
    #endregion

    private bool _isShowed = false;

    void Start()
    {
        if (PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) == 1)
            StartHandAnimation();
        else
            SetLevelText();
       
       _protocolText.text = $"{PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0)}";

       Settings.Instance.SetOutlines(Settings.Instance.Outlines == 1);

       _playButton.onClick.AddListener(PlayButton);
    }

    private void Update() 
    {
        if(!Input.GetMouseButtonDown(0)) return;

        if(!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Player"))) return;

        if(_settingsPanel.transform.localScale == Vector3.one || _upgradesCanvas.activeSelf) return;

        ShowEnergyCapacity();
         
    }
    
    public void UpgradesCanvasEnabled(bool status) 
    {
        Settings.Instance.PlayButtonSound();
        _upgradesCanvas.SetActive(status);
    }

    public void SetProtocolText() =>
        _protocolText.text = $"{PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0)}";

    #region Private Methods

    private void StartHandAnimation()
    { 
        _handRect.parent.gameObject.SetActive(true);
        _handRect.DOAnchorPos(_handRectYThreshold, _handRectMoveDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void SetLevelText()
    {
        _levelText.text = "Lab-" + (PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) - 1).ToString();
        _levelTextBlack.text = "Lab-" + (PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) - 1).ToString();
    }

    private void PlayButton()
    {
        Settings.Instance.PlayButtonSound();

        _escapeButton.SetActive(false);
        _settings.SettingsPanel.gameObject.SetActive(false);
        TransitionManager.Instance.PlayTransition(1f);
        Invoke(nameof(LoadLevel), .3f);

    } 

    private void LoadLevel()=>
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 1) + 1);

    private void ShowEnergyCapacity()
    {
        if(_isShowed) return;

        Settings.Instance.PlayButtonSound();

        _isShowed = true;

        _capacityRect.DOScale(Vector3.one, _showDuration);

        Invoke(nameof(CloseEnergyCapacity), _closeDuration);

        _capacityText.text = (_gameDesignData.MaxEnergyCapacity + (PlayerPrefs.GetInt(Consts.Prefs.CAPACITY, 0) * _gameDesignData.EnergyCapacityUpgradeAmount)).ToString();
    }

    private void CloseEnergyCapacity()=>
        _capacityRect.DOScale(Vector3.zero, _showDuration).OnComplete(()=> _isShowed = false);

    #endregion
}