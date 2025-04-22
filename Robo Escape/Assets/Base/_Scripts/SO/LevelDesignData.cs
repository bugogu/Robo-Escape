using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Scriptable Objects/LevelDesignData", order = 1)]
public class LevelDesignData : ScriptableObject
{
    [Header("General Settings")]
    public int chipsetCount = 4;
    [Tooltip("Seconds")] public float timeLimit = 90f;
    public bool waterLevel = false;
    [Tooltip("Seconds")] public float waterFillTime = 60;
    public string password;

    [Header("Mission Display")]
    public float typingSpeed = 0.035f; 
    public float delayBetweenMissions = 0.1f;
    public float closeDelay = 2f; 
}
