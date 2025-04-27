using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoSingleton<UIManager>
{
    public Button magneticPulseButton;

    [SerializeField] private GameDesignData _gameDesignData;

    [Header("General")]
    [SerializeField] private Button _homeButton;
    [SerializeField] private TMPro.TMP_Text _levelText;
    [SerializeField] private CanvasGroup _alarmImage;
    [SerializeField] private GameObject _waterCanvas;

    [Header("UI Elements")]
    [Tooltip("These element are goint to open when cutscene is finished")] 
    [SerializeField] private GameObject[] _uiElements;

    private float _loadDelay;

    void Start()
    {
        _levelText.text = PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) == 1 ? "Test-Lab" : "Lab-" + PlayerPrefs.GetInt(Consts.Prefs.LEVEL);
        _loadDelay = _gameDesignData.menuLoadDelay;
    }

    void OnEnable()
    {
        _homeButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(HomeButton);

        GameManager.Instance.OnAlarmSetted += AlarmImageAlpha;
        GameManager.Instance.OnGameStateChanged += OpenUI;
    }

    void OnDisable()
    {
        GameManager.Instance.OnAlarmSetted -= AlarmImageAlpha;
    }

    private void HomeButton()
    {
        TransitionManager.Instance.LoadLevel("Menu",_loadDelay);
    }

    private void AlarmImageAlpha(bool isAlarmActive)
    {
        if(isAlarmActive)
        _alarmImage.alpha = 1f;

        if(!isAlarmActive)
        _alarmImage.alpha = 0.3f;
    }

    private void OpenUI(GameState gameState)
    {
        if(gameState != GameState.Play) return;

        foreach (GameObject uiElement in _uiElements) uiElement.SetActive(true); 

        if(GameManager.Instance.waterLevel) _waterCanvas.SetActive(true);
    }
}
