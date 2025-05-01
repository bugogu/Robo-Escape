using System.Collections.Generic;
using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private Animator _levelEndAnimation;
    [SerializeField] private TMPro.TMP_Text _levelEndTitleText;
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private AudioClip _keyboardSound;
    [SerializeField] private List<GameObject> _levelEndClosingElements;
    [SerializeField] private GameObject _powerUpCounter;
    [SerializeField] private Image _powerUpFill;
    [SerializeField] private Color _shieldPowerUpColor;
    [SerializeField] private Color _flashPowerUpColor;
    [SerializeField] private ParticleSystem _levelEndConfetti;

    [Header("UI Elements")]
    [Tooltip("These element are goint to open when cutscene is finished")] 
    [SerializeField] private GameObject[] _uiElements;

    private float _loadDelay;
    private AudioSource _audioSource;

    void Start()
    {
        _levelText.text = PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) == 1 ? "Test-Lab" : "Lab-" + PlayerPrefs.GetInt(Consts.Prefs.LEVEL);
        _loadDelay = _gameDesignData.menuLoadDelay;

        _audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        _homeButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(HomeButton);

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

    private void HomeButton()
    {
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

    private System.Collections.IEnumerator TypeTextForTitle(string text)
    {
        _levelEndTitleText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            _levelEndTitleText.text += letter; 

            if (letter != ' ' && letter != '\n' && _audioSource != null && _keyboardSound != null)
                _audioSource.PlayOneShot(_keyboardSound); 

            yield return new WaitForSeconds(_typingSpeed); 
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
}
