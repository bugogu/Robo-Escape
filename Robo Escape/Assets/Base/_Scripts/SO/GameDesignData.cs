using UnityEngine;

[CreateAssetMenu(fileName = "GameDesignData", menuName = "Game/Design Data")]
public class GameDesignData : ScriptableObject
{
    [Header("Character Movement Settings")]
    public float characterRunSpeed;
    public float characterWalkSpeed;
    public float characterTurnSpeed;
    public float magnetismSpeedMultiplier = 2f; 

    [Header("Character Energy Consumption Settings")]
    public float replenishAmount = 1f;
    public float consumeAmount = 0.3f;
    public float magnetismConsumptionMultiplier = 1.5f;
    public float magnetismReplenishMultiplier = 2f;
    public float maxEnergyCapacity = 150f;

    [Header("Energy PopUp Text")]
    public float moveDistance = 100f;
    public float moveDuration = 0.5f;
    public float returnDuration = 0.3f;
    public float displayDuration = 0.2f;
    public Color _positiveColor, _negativeColor;



}
