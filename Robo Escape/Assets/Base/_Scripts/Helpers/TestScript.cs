using UnityEngine;
using UnityEditor;

public class TestScript : MonoBehaviour
{
    
#if UNITY_EDITOR
    
    [MenuItem("Test Methods/ConsumeEnergy")]
    private static void ConsumeEnergy()
    {
        EnergyBar.Instance.ConsumeEnergy(1f);
    }

    [MenuItem("Test Methods/ReplenishEnergy")]
    private static void ReplenishEnergy()
    {
        EnergyBar.Instance.ReplenishEnergy(1f);
    }

#endif

}
