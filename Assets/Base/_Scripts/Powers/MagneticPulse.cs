using UnityEngine;

public class MagneticPulse : MonoBehaviour
{
    public void ActivateMagneticCharge() =>
        Player.PlayerController.Instance.GainPowerUp(PowerUpType.MagneticPulse);
}
