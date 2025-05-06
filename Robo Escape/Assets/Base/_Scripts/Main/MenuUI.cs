using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuUI : MonoSingleton<MenuUI>
{
    [SerializeField] private GameDesignData _gameDesignData;
    [SerializeField] private TMPro.TMP_Text _levelText;
    [SerializeField] private TMPro.TMP_Text _protocolText;
    [SerializeField] private Button _playButton;
    [SerializeField] private GameObject _escapeButton;
    [SerializeField] private Settings _settings;
    [SerializeField] private RectTransform _capacityRect;
    [SerializeField] private TMPro.TMP_Text _capacityText;
    [SerializeField] private float _showDuration = 1f;
    [SerializeField] private float _closeDuration = 3f;
    [SerializeField] private RectTransform _handRect;
    [SerializeField] private Vector3 _handRectYThreshold = new Vector3(-140, 160, 0);
    [SerializeField] private float _handRectMoveDuration = 1f;

    private bool _isShowed = false;

    void Start()
    {
        if(PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) == 1)
        {
            _levelText.text = "Test-Lab";
            _handRect.parent.gameObject.SetActive(true);
            _handRect.DOAnchorPos(_handRectYThreshold, _handRectMoveDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
        
        else
       _levelText.text = "Lab-" + (PlayerPrefs.GetInt("Level", 1) -1).ToString();  
       
       _protocolText.text = $"{PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0)}";

       Settings.Instance.SetOutlines(Settings.Instance.Outlines == 1);

       _playButton.onClick.AddListener(PlayButton);
    }

    private void Update() 
    {
        if(!Input.GetMouseButtonDown(0)) return;

        if(!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100, LayerMask.GetMask("Player"))) return;

        ShowEnergyCapacity();
         
    }
    private void PlayButton() 
    {
        _escapeButton.SetActive(false);
        _settings._settingsPanel.gameObject.SetActive(false);
        TransitionManager.Instance.PlayTransition(1f);
        Invoke(nameof(LoadLevel), 0.3f);
        
    } 

    private void LoadLevel()=>
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 1) + 1);

    private void ShowEnergyCapacity()
    {
        if(_isShowed) return;

        _isShowed = true;

        _capacityRect.DOScale(Vector3.one, _showDuration);

        Invoke(nameof(CloseEnergyCapacity), _closeDuration);

        _capacityText.text = _gameDesignData.maxEnergyCapacity.ToString();
    }

    private void CloseEnergyCapacity()=>
        _capacityRect.DOScale(Vector3.zero, _showDuration).OnComplete(()=> _isShowed = false);
}
