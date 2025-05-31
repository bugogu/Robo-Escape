#region Libraries

using System.Collections.Generic;
using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;
using UnityEngine.Serialization;

#endregion

[DefaultExecutionOrder(-1)]
public class UIManager : MonoSingleton<UIManager>
{
    #region References

    [Header("Buttons")]
    [FormerlySerializedAs("magneticPulseButton")] public Button MagneticPulseButton;
    [SerializeField] private Button _nextLevelButton, _menuButton, _tryAgainButton, _homeButton;

    [Header("Texts")]
    [SerializeField] private TMP_Text _levelEndTitleText;
    [SerializeField] private TMP_Text _missionChipsetsText, _missionTimeText, _missionAlarmText, _levelText;

    [Header("GameObjects")]
    [SerializeField] private GameObject _waterCanvas;
    [SerializeField] private GameObject _powerUpCounter, _levelEndMissionCanvas;
    
    [Header("Colors")]
    [SerializeField] private Color _shieldPowerUpColor;
    [SerializeField] private Color _flashPowerUpColor, _completedMissionColor, _failedMissionColor;    

    [Header("Others")]
    [SerializeField] private GameDesignData _gameDesignData;
    [SerializeField] private CanvasGroup _alarmImage;
    [SerializeField] private Animator _levelEndAnimation;
    [SerializeField] private float _typingSpeed = .05f;
    [SerializeField] private AudioClip _keyboardSound;
    [SerializeField] private List<GameObject> _levelEndClosingElements;
    [SerializeField] private Image _powerUpFill;
    [SerializeField] private ParticleSystem _levelEndConfetti;
    [Tooltip("These elements are going to open when cutscene is finished")] 
    [SerializeField] private GameObject[] _uiElements;

    #endregion

    #region Private Fields

    private float _loadDelay;
    private AudioSource _audioSource;
    private List<string> _missions;

    #endregion

    #region Unity Events

    void Start()
    {
        _levelText.text = "Lab-" + PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1).ToString();
        _loadDelay = _gameDesignData.MenuLoadDelay;

        _audioSource = GetComponent<AudioSource>();

        _missions = new List<string>()
        {
            $"Collect {LevelManager.Instance.ChipsetCount} chipsets!",  
            $"Finish under {LevelManager.Instance.TimeLimit} sec!", 
            "No alarm allowed!"                           
        };
    }

    void OnEnable()
    {
        SetOnClicks();
        
        GameManager.Instance.OnAlarmSetted += AlarmImageAlpha;
        GameManager.Instance.OnGameStateChanged += OpenUI;
        GameManager.Instance.OnGameStateChanged += PlayLevelEndAnimation;
    }

    void OnDisable()
    {
        GameManager.Instance.OnAlarmSetted -= AlarmImageAlpha;
        GameManager.Instance.OnGameStateChanged -= OpenUI;
        GameManager.Instance.OnGameStateChanged -= PlayLevelEndAnimation;
    }

    #endregion 

    public void ActivatePowerCounter(float time, bool isShield)
    {
        if(isShield) _powerUpFill.color = _shieldPowerUpColor;
        else _powerUpFill.color = _flashPowerUpColor;

        _powerUpCounter.SetActive(true);
        
        _powerUpFill.FillImageAnimation(1,0,time).SetEase(Ease.Linear).OnComplete(()=> ResetPowerCounter());
    }

    public void TryAgainButton()
    {
        if(GameManager.Instance.IsAlarmActive && Settings.Instance.Music == 1)
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();

        Settings.Instance.PlayButtonSound();

        TransitionManager.Instance.PlayTransition(1f);
        Invoke(nameof(LoadLevel), 0.3f);
        _levelEndMissionCanvas.SetActive(false);
        _levelEndAnimation.gameObject.SetActive(false);

        if(_homeButton.transform.parent.gameObject.activeInHierarchy) _homeButton.transform.parent.gameObject.SetActive(false);
    }

    #region Private Methods

    private void SetOnClicks()
    {
        _homeButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(HomeButton);

        _menuButton.onClick.RemoveAllListeners();
        _menuButton.onClick.AddListener(MenuButton);

        _tryAgainButton.onClick.RemoveAllListeners();
        _tryAgainButton.onClick.AddListener(TryAgainButton);

        _nextLevelButton.onClick.RemoveAllListeners();
        _nextLevelButton.onClick.AddListener(NextLevelButton);
    }

    private void HomeButton()
    {
        Settings.Instance.PlayButtonSound();

        if(GameManager.Instance.IsAlarmActive && Settings.Instance.Music == 1)
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();

        TransitionManager.Instance.LoadLevel("Menu",_loadDelay);
    }

    private void AlarmImageAlpha(bool IsAlarmActive)
    {
        if(IsAlarmActive)
        _alarmImage.alpha = 1f;

        if(!IsAlarmActive)
        _alarmImage.alpha = 0.3f;
    }

    private void OpenUI(GameState gameState)
    {
        if(gameState != GameState.Play) return;

        foreach (GameObject uiElement in _uiElements) uiElement.SetActive(true); 

        if(GameManager.Instance.WaterLevel) _waterCanvas.SetActive(true);
    }

    private void PlayLevelEndAnimation(GameState gameState)
    {
        if (gameState == GameState.Win)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.WinSfx);
            SetText($"Escaped From {_levelText.text}");
        }
            
        else if (gameState == GameState.Lose)
        {   
            SoundManager.Instance.PlaySFX(SoundManager.Instance.LoseSfx);
            SetText("Try Again?");
        }
        else
            return;

        _levelEndConfetti.Play();

        foreach (GameObject element in _levelEndClosingElements) element.SetActive(false);
        
        _levelEndAnimation.gameObject.SetActive(true);
    }

    private void SetText(string text)
    {
        StartCoroutine(TypeTextForTitle(text));
    }

    private IEnumerator TypeTextForTitle(string text)
    {
        yield return new WaitForSeconds(1f);

        _levelEndTitleText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            _levelEndTitleText.text += letter;

            if (letter != ' ' && letter != '\n' && _audioSource != null && _keyboardSound != null)
                _audioSource.PlayOneShot(_keyboardSound);

            yield return new WaitForSeconds(_typingSpeed);
        }

        StartCoroutine(HandleLevelEndPanelUIElements());
        
    }

    private IEnumerator HandleLevelEndPanelUIElements()
    { 
        if(GameManager.Instance.GetCurrentState() == GameState.Lose)
        {
            _tryAgainButton.gameObject.SetActive(true);
            _tryAgainButton.transform.DOPunchScale(_tryAgainButton.transform.localScale ,.5f,5,10);

            yield return new WaitForSeconds(.3f);

            _menuButton.gameObject.SetActive(true);
            _menuButton.transform.DOPunchScale(_menuButton.transform.localScale ,.5f,5,10);
        }

        if(GameManager.Instance.GetCurrentState() == GameState.Win)
        {
            _nextLevelButton.gameObject.SetActive(true);
            _nextLevelButton.transform.DOPunchScale(_nextLevelButton.transform.localScale ,.5f,5,10);

            yield return new WaitForSeconds(.3f);

            _menuButton.gameObject.SetActive(true);
            _menuButton.transform.DOPunchScale(_menuButton.transform.localScale ,.5f,5,10);

            yield return new WaitForSeconds(1f);

            _levelEndMissionCanvas.SetActive(true);
            _missionChipsetsText.color = LevelManager.Instance.ChipsetMissionCompleted ? _completedMissionColor : _failedMissionColor;
            _missionTimeText.color = LevelManager.Instance.TimeMissionCompleted ? _completedMissionColor : _failedMissionColor;
            _missionAlarmText.color = LevelManager.Instance.AlarmMissionCompleted ? _completedMissionColor : _failedMissionColor;
            StartCoroutine(ShowMissions(_missions));
        }
    }

    private void ResetPowerCounter()
    {
        _powerUpCounter.SetActive(false);
        _powerUpFill.fillAmount = 1f;
    }

    private IEnumerator ShowMissions(List<string> missions)
    {
        // 1. Görev (Chipsets)
        if(missions.Count > 0 && _missionChipsetsText != null)
        {
            yield return StartCoroutine(TypeText(missions[0], _missionChipsetsText));
            yield return new WaitForSeconds(0.1f);
        }

        // 2. Görev (Time)
        if(missions.Count > 1 && _missionTimeText != null)
        {
            yield return StartCoroutine(TypeText(missions[1], _missionTimeText));
            yield return new WaitForSeconds(0.1f);
        }

        // 3. Görev (Alarm)
        if(missions.Count > 2 && _missionAlarmText != null)
        {
            yield return StartCoroutine(TypeText(missions[2], _missionAlarmText));
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator TypeText(string text, TMP_Text targetText)
    {
        targetText.text = "";
        
        foreach (char letter in text.ToCharArray())
        {
            targetText.text += letter;

            if (letter != ' ' && letter != '\n' && _audioSource != null && _keyboardSound != null)
                _audioSource.PlayOneShot(_keyboardSound);
                
            yield return new WaitForSeconds(_typingSpeed);
        }
    }

    private void MenuButton()
    {
        Settings.Instance.PlayButtonSound();
        
        if(GameManager.Instance.IsAlarmActive && Settings.Instance.Music == 1)
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();

        if (GameManager.Instance.GetCurrentState() == GameState.Win)
        {            
            var currentLevel = PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1);
            var maxLevel = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2;

            var newLevel = currentLevel >= maxLevel ? 1 : currentLevel + 1;

            PlayerPrefs.SetInt(Consts.Prefs.LEVEL, newLevel);
            TransitionManager.Instance.LoadLevel("Menu", _loadDelay);
        }
        
        if (GameManager.Instance.GetCurrentState() == GameState.Lose)
            TransitionManager.Instance.LoadLevel("Menu", _loadDelay);

        _levelEndMissionCanvas.SetActive(false);
        _levelEndAnimation.gameObject.SetActive(false);
    }

    private void NextLevelButton()
    {
        Settings.Instance.PlayButtonSound();

        var currentLevel = PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1);
        var maxLevel = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2;
        var newLevel = currentLevel >= maxLevel ? 1 : currentLevel + 1;

        PlayerPrefs.SetInt(Consts.Prefs.LEVEL, newLevel);
        TransitionManager.Instance.PlayTransition(1f);
        Invoke(nameof(LoadLevel), 0.3f);
        _levelEndMissionCanvas.SetActive(false);
        _levelEndAnimation.gameObject.SetActive(false);
    }

    private void LoadLevel()=>
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 1) + 1);

    #endregion
}