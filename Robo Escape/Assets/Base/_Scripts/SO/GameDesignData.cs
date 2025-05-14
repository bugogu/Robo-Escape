using UnityEngine;

[CreateAssetMenu(fileName = "GameDesignData", menuName = "Scriptable Objects/Game Design Data")]
public class GameDesignData : ScriptableObject
{
    [Header("Player Movement Settings"), Space(20)]
    public float CharacterRunSpeed;
    public float CharacterWalkSpeed;
    public float CharacterTurnSpeed;
    public float MagnetismSpeedMultiplier = 2f; 
    public float MovementThreshold = 0.35f;
    public float StopThreshold = 0.25f;

    [Header("Player Energy Consumption Settings"), Space(20)]
    public float ReplenishAmount = 1f;
    public float ConsumeAmount = 0.3f;
    public float MagnetismConsumptionMultiplier = 1.5f;
    public float MagnetismReplenishMultiplier = 2f;
    public float MaxEnergyCapacity = 150f;

    [Header("Player Power Ups Settings"), Space(20)]
    public float FlashSpeedMultiplier = 2f;
    public float FlashPowerUpDuration = 10f;
    public Color FlashOutlineColor, ShieldOutlineColor, EmpOutlineColor;

    [Header("Energy PopUp Text Settings"), Space(20)]
    public float MoveDistance = 100f;
    public float MoveDuration = 0.5f;
    public float ReturnDuration = 0.3f;
    public float DisplayDuration = 0.2f;
    public Color PositiveColor, NegativeColor;

    [Header("Camera Sahke Settings"), Space(20)]
    public float CameraShakeDuration; 
    public float CameraShakeAmplitude; 
    public float CameraShakeFrequency;

    [Header("UI General Settings"), Space(20)]
    public float MenuLoadDelay = 0.1f;

    [Header("Upgrade Settings"), Space(20)]
    public float EnergyCapacityUpgradeAmount = 5f;



}
