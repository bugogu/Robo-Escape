using UnityEngine;

public class EnergyCell : MonoBehaviour, ICollectable
{
    [SerializeField] private float _replenishAmount = 35f;
    public bool Collect()
    {
        EnergyBar.Instance.ReplenishEnergy(_replenishAmount , true);
        gameObject.SetActive(false);
        return true;
    }
}
