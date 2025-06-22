using UnityEngine;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Scriptable Objects/UpgradeElementData", order = 1)]
public class UpgradeElementData : ScriptableObject
{
    public int Price;
    public int MaxIncreaseCount;
    public string Title;
}
