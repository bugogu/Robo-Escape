using UnityEngine;

public class AntiAlarm : MonoBehaviour
{
    public void ActivateShield()
    {
        PlayerController.Instance.GainPowerUp(PowerUpType.Shield);
    }
}
