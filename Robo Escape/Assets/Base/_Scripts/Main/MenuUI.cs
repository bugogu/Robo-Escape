using MaskTransitions;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoSingleton<MenuUI>
{
    [SerializeField] private TMPro.TMP_Text _levelText;
    [SerializeField] private Button _playButton;

    void Start()
    {
       _levelText.text = "Lab-" + PlayerPrefs.GetInt("Level", 1);  
       _playButton.onClick.AddListener(PlayButton);
    }

    private void PlayButton() 
    {
        TransitionManager.Instance.PlayTransition(1f);
        Invoke(nameof(LoadLevel), 0.3f);
        
    } 

    private void LoadLevel()=>
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 1) + 1);
        
}
