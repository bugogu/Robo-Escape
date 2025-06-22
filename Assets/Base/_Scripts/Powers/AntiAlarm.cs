using UnityEngine;

public class AntiAlarm : MonoBehaviour
{
    public void ActivateShield() =>
        Player.PlayerController.Instance.GainPowerUp(PowerUpType.Shield);
}
