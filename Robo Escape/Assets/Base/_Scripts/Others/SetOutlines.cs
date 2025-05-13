using UnityEngine;

public class SetOutlines : MonoBehaviour
{
    [SerializeField] private string layerName = "";
    void OnEnable()
    {
        Settings.Instance.OnOutlinesSetted += OutlineEnabled;
    }

    void OnDisable()
    {
        if (Settings.Instance != null)
            Settings.Instance.OnOutlinesSetted -= OutlineEnabled;
    }

    private void OutlineEnabled(bool status)
    {
        gameObject.layer = status ? LayerMask.NameToLayer(layerName) : LayerMask.NameToLayer("Default");
    }
}
