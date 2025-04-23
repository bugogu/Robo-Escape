using UnityEngine;

public class Flash : MonoBehaviour
{
    public void ActivateFlash()
    {
        PlayerController.Instance.GainPowerUp(PowerUpType.Flash);
        EnergyBar.Instance.ReplenishEnergy(EnergyBar.Instance._maxEnergyCapacity);
    }
}
