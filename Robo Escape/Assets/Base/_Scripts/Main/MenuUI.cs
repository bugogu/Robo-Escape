using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoSingleton<MenuUI>
{
    [SerializeField] private TMPro.TMP_Text _levelText;
    [SerializeField] private TMPro.TMP_Text _protocolText;
    [SerializeField] private Button _playButton;
    [SerializeField] private GameObject _escapeButton;
    [SerializeField] private Settings _settings;

    void Start()
    {
        if(PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) == 1)
        _levelText.text = "Test-Lab";
        else
       _levelText.text = "Lab-" + (PlayerPrefs.GetInt("Level", 1) -1).ToString();  
       
       _protocolText.text = $"{PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0)}";

       Settings.Instance.SetOutlines(Settings.Instance.Outlines == 1);

       _playButton.onClick.AddListener(PlayButton);
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
        
}
