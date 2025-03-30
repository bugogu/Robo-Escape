using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoSingleton<UIManager>
{
    [Header("Home Button")]
    [SerializeField] private Button _homeButton;
    [SerializeField] private float _loadDelay = 1f;

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
