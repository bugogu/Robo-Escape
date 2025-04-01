using UnityEngine;

public class DrainCell : MonoBehaviour, ICollectable
{
    [SerializeField] private float _consumeAmount = 35f;
    public bool Collect()
    {
        EnergyBar.Instance.ConsumeEnergy(_consumeAmount);
        gameObject.SetActive(false);
        return false;
    }
}
