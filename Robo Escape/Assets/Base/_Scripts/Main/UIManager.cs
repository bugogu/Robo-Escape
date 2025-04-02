using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoSingleton<UIManager>
{
    [Header("Home Button")]
    [SerializeField] private Button _homeButton;
    [SerializeField] private float _loadDelay = 1f;
    [SerializeField] private TMPro.TMP_Text _levelText;

    void Start()
    {
        _levelText.text = "Lab-" + PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1);
    }

    void OnEnable()
    {
        _homeButton.onClick.RemoveAllListeners();
        _homeButton.onClick.AddListener(HomeButton);
    }

    private void HomeButton()
    {
        TransitionManager.Instance.LoadLevel("Menu",_loadDelay);
    }
}
