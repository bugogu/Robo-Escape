using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MissionDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text missionText;
    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip keyboardSound;

    private float _typingSpeed; 
    private float _delayBetweenMissions;
    private float _closeDelay; 

    private void Start()
    {
        _typingSpeed = LevelManager.Instance.levelData.typingSpeed;
        _delayBetweenMissions = LevelManager.Instance.levelData.delayBetweenMissions;
        _closeDelay = LevelManager.Instance.levelData.closeDelay;

        missionText.text = "";
        
        List<string> missions = new List<string>()
        {
            $"Collect {LevelManager.Instance.chipsetCount} chipsets!",  
            $"Finish under {LevelManager.Instance._timeLimit} sec!", 
            "No alarm allowed!"                           
        };

        StartCoroutine(ShowMissions(missions));
    }

    IEnumerator ShowMissions(List<string> missions)
    {
        missionText.transform.parent.gameObject.SetActive(true); 

        foreach (string mission in missions)
        {
            string currentMission = "- " + mission + "\n"; 
            yield return StartCoroutine(TypeText(currentMission)); 
            yield return new WaitForSeconds(_delayBetweenMissions); 
        }

        yield return new WaitForSeconds(_closeDelay);
        missionText.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator TypeText(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            missionText.text += letter; 

            if (letter != ' ' && letter != '\n' && audioSource != null && keyboardSound != null)
                audioSource.PlayOneShot(keyboardSound); 
                
            yield return new WaitForSeconds(_typingSpeed); 
        }
    }
}
