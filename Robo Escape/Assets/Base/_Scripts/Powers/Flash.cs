using UnityEngine;

public class Flash : MonoBehaviour
{
    public void ActivateFlash()
    {
        Player.PlayerController.Instance.GainPowerUp(PowerUpType.Flash);
        EnergyBar.Instance.ReplenishEnergy(EnergyBar.Instance.MaxEnergyCapacity);
    }
}
