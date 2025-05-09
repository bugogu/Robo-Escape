using System.Collections.Generic;
using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoSingleton<UIManager>
{
    public Button magneticPulseButton;

    [SerializeField] private GameDesignData _gameDesignData;

    [Header("General")]
    [SerializeField] private Button _homeButton;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private CanvasGroup _alarmImage;
    [SerializeField] private GameObject _waterCanvas;
    [SerializeField] private Animator _levelEndAnimation;
    [SerializeField] private TMP_Text _levelEndTitleText;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private AudioClip _keyboardSound;
    [SerializeField] private List<GameObject> _levelEndClosingElements;
    [SerializeField] private GameObject _powerUpCounter;
    [SerializeField] private Image _powerUpFill;
    [SerializeField] private Color _shieldPowerUpColor;
    [SerializeField] private Color _flashPowerUpColor;
    [SerializeField] private ParticleSystem _levelEndConfetti;
    [SerializeField] private Color _completedMissionColor;
    [SerializeField] private Color _failedMissionColor;
    [SerializeField] private TMP_Text _missionChipsetsText, _missionTimeText, _missionAlarmText;
    [SerializeField] private GameObject _levelEndMissionCanvas;
    [SerializeField] private Button _nextLevelButton, _menuButton, _tryAgainButton;
    
    [Header("UI Elements")]
    [Tooltip("These element are going to open when cutscene is finished")] 
    [SerializeField] private GameObject[] _uiElements;

    private float _loadDelay;
    private AudioSource _audioSource;
    private List<string> _missions;

    void Start()
    {
        _levelText.text = PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) == 1 ? "Test-Lab" : "Lab-" + (PlayerPrefs.GetInt(Consts.Prefs.LEVEL) - 1).ToString();
        _loadDelay = _gameDesignData.menuLoadDelay;

        _audioSource = GetComponent<AudioSource>();

        _missions = new List<string>()
        {
            $"Collect {LevelManager.Instance.chipsetCount} chipsets!",  
            $"Finish under {LevelManager.Instance._timeLimit} sec!", 
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

        if(GameManager.Instance.isAlarmActive && Settings.Instance.Music == 1)
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();

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

    private void PlayLevelEndAnimation(GameState gameState)
    {
        if(gameState == GameState.Win)
            SetText($"Escaped From {_levelText.text}");
        else if(gameState == GameState.Lose)
            SetText("Try Again?");
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
        _levelEndTitleText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            _levelEndTitleText.text += letter; 

            if (letter != ' ' && letter != '\n' && _audioSource != null && _keyboardSound != null)
                _audioSource.PlayOneShot(_keyboardSound); 

            yield return new WaitForSeconds(_typingSpeed); 
        }

        if(PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) == 1)
        {
            _nextLevelButton.gameObject.SetActive(true);
            _nextLevelButton.transform.DOPunchScale(_nextLevelButton.transform.localScale ,0.5f,5,10);
            
            yield return new WaitForSeconds(1f);

            _menuButton.gameObject.SetActive(true);
            _menuButton.transform.DOPunchScale(_menuButton.transform.localScale ,0.5f,5,10);
            
        }

        if(GameManager.Instance.GetCurrentState() == GameState.Lose)
        {
            _tryAgainButton.gameObject.SetActive(true);
            _tryAgainButton.transform.DOPunchScale(_tryAgainButton.transform.localScale ,0.5f,5,10);

            yield return new WaitForSeconds(1f);

            _menuButton.gameObject.SetActive(true);
            _menuButton.transform.DOPunchScale(_menuButton.transform.localScale ,0.5f,5,10);
        }

        if(GameManager.Instance.GetCurrentState() == GameState.Win)
        {
            if(PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) != 1)
            {
                _levelEndMissionCanvas.SetActive(true);

                _missionChipsetsText.color = LevelManager.Instance.chipsetMissionCompleted ? _completedMissionColor : _failedMissionColor;
                _missionTimeText.color = LevelManager.Instance.timeMissionCompleted ? _completedMissionColor : _failedMissionColor;
                _missionAlarmText.color = LevelManager.Instance.alarmMissionCompleted ? _completedMissionColor : _failedMissionColor;

                StartCoroutine(ShowMissions(_missions));
            }
        }
    }

    public void ActivatePowerCounter(float time, bool isShield)
    {
        if(isShield) _powerUpFill.color = _shieldPowerUpColor;
        else _powerUpFill.color = _flashPowerUpColor;

        _powerUpCounter.SetActive(true);
        
        _powerUpFill.FillImageAnimation(1,0,time).SetEase(Ease.Linear).OnComplete(()=> ResetPowerCounter());
    }

    private void ResetPowerCounter()
    {
        _powerUpCounter.SetActive(false);
        _powerUpFill.fillAmount = 1f;
    }

    IEnumerator ShowMissions(List<string> missions)
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

        _nextLevelButton.gameObject.SetActive(true);
        _nextLevelButton.transform.DOPunchScale(_nextLevelButton.transform.localScale ,0.5f,5,10);

        yield return new WaitForSeconds(1f);

        _menuButton.gameObject.SetActive(true);
        _menuButton.transform.DOPunchScale(_menuButton.transform.localScale ,0.5f,5,10);

    }

    IEnumerator TypeText(string text, TMP_Text targetText)
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
        
        if(GameManager.Instance.isAlarmActive && Settings.Instance.Music == 1)
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().Play();

        if(GameManager.Instance.GetCurrentState() == GameState.Win)
        {
            PlayerPrefs.SetInt(Consts.Prefs.LEVEL, PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) + 1);
            TransitionManager.Instance.LoadLevel("Menu",_loadDelay);
        }

        if(GameManager.Instance.GetCurrentState() == GameState.Lose)
        TransitionManager.Instance.LoadLevel("Menu",_loadDelay);

        _levelEndMissionCanvas.SetActive(false);
        _levelEndAnimation.gameObject.SetActive(false);
    }

    private void NextLevelButton()
    {
        Settings.Instance.PlayButtonSound();

        PlayerPrefs.SetInt(Consts.Prefs.LEVEL, PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) + 1);
        TransitionManager.Instance.PlayTransition(1f);
        Invoke(nameof(LoadLevel), 0.3f);
        _levelEndMissionCanvas.SetActive(false);
        _levelEndAnimation.gameObject.SetActive(false);
    }

    private void TryAgainButton()
    {
        Settings.Instance.PlayButtonSound();

        TransitionManager.Instance.PlayTransition(1f);
        Invoke(nameof(LoadLevel), 0.3f);
        _levelEndMissionCanvas.SetActive(false);
        _levelEndAnimation.gameObject.SetActive(false);
    }

    private void LoadLevel()=>
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 1) + 1);
}
