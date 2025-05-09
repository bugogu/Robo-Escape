using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LevelDesignData", menuName = "Scriptable Objects/UpgradeElementData", order = 1)]
public class UpgradeElementData : ScriptableObject
{
    public int price;
    public Sprite icon;
    public UnityEvent onBuy;
}
