using UnityEngine;

[CreateAssetMenu(fileName = "GameDesignData", menuName = "Scriptable Objects/Game Design Data")]
public class GameDesignData : ScriptableObject
{
    [Header("Player Movement Settings")]
    public float characterRunSpeed;
    public float characterWalkSpeed;
    public float characterTurnSpeed;
    public float magnetismSpeedMultiplier = 2f; 
    public float movementThreshold = 0.35f;
    public float stopThreshold = 0.25f;

    [Header("Player Energy Consumption Settings")]
    public float replenishAmount = 1f;
    public float consumeAmount = 0.3f;
    public float magnetismConsumptionMultiplier = 1.5f;
    public float magnetismReplenishMultiplier = 2f;
    public float maxEnergyCapacity = 150f;

    [Header("Player Power Ups Settings")]
    public float flashSpeedMultiplier = 2f;
    public float flashPowerUpDuration = 10f;
    public Color flashOutlineColor;
    public Color shieldOutlineColor;
    public Color empOutlineColor;

    [Header("Energy PopUp Text")]
    public float moveDistance = 100f;
    public float moveDuration = 0.5f;
    public float returnDuration = 0.3f;
    public float displayDuration = 0.2f;
    public Color _positiveColor, _negativeColor;

    [Header("UI General")]
    public float menuLoadDelay = 0.1f;



}
