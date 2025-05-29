using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Scriptable Objects/LevelDesignData", order = 1)]
public class LevelDesignData : ScriptableObject
{
    [Header("General Settings")]
    public int ChipsetCount = 4;
    [Tooltip("Seconds")] public float TimeLimit = 90f;
    public bool WaterLevel = false;
    [Tooltip("Seconds")] public float WaterFillTime = 60;
    public string Password;

    [Header("Mission Display")]
    public float TypingSpeed = .035f; 
    public float DelayBetweenMissions = .1f;
    public float CloseDelay = 2f; 
}
