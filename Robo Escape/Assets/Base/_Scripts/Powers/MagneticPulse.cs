using UnityEngine;

public class MagneticPulse : MonoBehaviour
{
    public void ActivateMagneticCharge()
    {
        PlayerController.Instance.GainPowerUp(PowerUpType.MagneticPulse);
    }
}
