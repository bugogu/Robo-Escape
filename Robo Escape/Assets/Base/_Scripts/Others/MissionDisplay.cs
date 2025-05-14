using UnityEngine;
using UnityEngine.Serialization;

public class MissionDisplay : MonoBehaviour
{
    [FormerlySerializedAs("missionText")] [ SerializeField] private TMPro.TMP_Text _missionText;
    [Header("Sound Settings")]
    [FormerlySerializedAs("audioSource"), SerializeField] private AudioSource _audioSource;
    [FormerlySerializedAs("keyboardSound"), SerializeField] private AudioClip _keyboardSound;

    private System.Collections.Generic.List<string> _missions;
    private float _typingSpeed; 
    private float _delayBetweenMissions;
    private float _closeDelay; 

    private void Start()
    {
        if(PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) == 1)
		{
			gameObject.SetActive(false);
			return;
        }
        _typingSpeed = LevelManager.Instance.LevelData.TypingSpeed;
        _delayBetweenMissions = LevelManager.Instance.LevelData.DelayBetweenMissions;
        _closeDelay = LevelManager.Instance.LevelData.CloseDelay;

        _missionText.text = "";
        
        _missions = new System.Collections.Generic.List<string>()
        {
            $"Collect {LevelManager.Instance.ChipsetCount} chipsets!",  
            $"Finish under {LevelManager.Instance.TimeLimit} sec!", 
            "No alarm allowed!"                           
        };

        StartCoroutine(ShowMissions(_missions));
    }

    System.Collections.IEnumerator ShowMissions(System.Collections.Generic.List<string> missions)
    {
        _missionText.transform.parent.gameObject.SetActive(true); 

        foreach (string mission in missions)
        {
            string currentMission = "- " + mission + "\n"; 
            yield return StartCoroutine(TypeText(currentMission)); 
            yield return new WaitForSeconds(_delayBetweenMissions); 
        }

        yield return new WaitForSeconds(_closeDelay);
        _missionText.transform.parent.gameObject.SetActive(false);
    }

    System.Collections.IEnumerator TypeText(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            _missionText.text += letter; 

            if (letter != ' ' && letter != '\n' && _audioSource != null && _keyboardSound != null)
                _audioSource.PlayOneShot(_keyboardSound); 
                
            yield return new WaitForSeconds(_typingSpeed); 
        }
    }
}
