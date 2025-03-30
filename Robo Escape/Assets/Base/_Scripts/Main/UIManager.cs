using UnityEngine;
[DefaultExecutionOrder(-1)]
public class UIManager : MonoSingleton<UIManager>
{
    public int Level 
    {
        get => PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1);
        set => PlayerPrefs.SetInt(Consts.Prefs.LEVEL, value);
    }
}
