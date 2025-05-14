using UnityEngine;
using UnityEngine.Serialization;

public class SetOutlines : MonoBehaviour
{
    [FormerlySerializedAs("layerName")] [ SerializeField] private string _layerName = "";

    void OnEnable()
    {
        if (Settings.Instance != null)
            Settings.Instance.OnOutlinesSetted += OutlineEnabled;
    }

    void OnDisable()
    {
        if (Settings.Instance != null)
            Settings.Instance.OnOutlinesSetted -= OutlineEnabled;
    }

    private void OutlineEnabled(bool status)
    {
        gameObject.layer = status ? LayerMask.NameToLayer(_layerName) : LayerMask.NameToLayer("Default");
    }
}
