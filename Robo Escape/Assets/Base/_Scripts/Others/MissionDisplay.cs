using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MissionDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text missionText;
    [SerializeField] private float typingSpeed = 0.05f; 
    [SerializeField] private float delayBetweenMissions = 0.5f;
    [SerializeField] private float closeDelay = 2f; 

    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip keyboardSound;

    private void Start()
    {
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
            yield return new WaitForSeconds(delayBetweenMissions); 
        }

        yield return new WaitForSeconds(closeDelay);
        missionText.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator TypeText(string text)
    {
        foreach (char letter in text.ToCharArray())
        {
            missionText.text += letter; 

            if (letter != ' ' && letter != '\n' && audioSource != null && keyboardSound != null)
                audioSource.PlayOneShot(keyboardSound); 
                
            yield return new WaitForSeconds(typingSpeed); 
        }
    }
}
