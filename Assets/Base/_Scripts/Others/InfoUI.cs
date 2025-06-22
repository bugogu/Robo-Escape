using UnityEngine;

public class InfoUI : MonoBehaviour
{
    [SerializeField] private GameObject _lockScreen, _infoPanel, _hand;
    [SerializeField] private Canvas _upgradeCanvas;

    void Start()
    {
        if (PlayerPrefs.GetInt(Consts.Prefs.LEVEL, 1) != 1) _lockScreen.SetActive(false);
    }

    public void InfoPanelEnabled()
    {
        Settings.Instance.PlayButtonSound();
        _infoPanel.SetActive(!_infoPanel.activeSelf);

        _infoPanel.transform.parent.GetComponent<Canvas>().sortingOrder = _upgradeCanvas.sortingOrder + 1;

        _lockScreen.SetActive(false);
        _hand.SetActive(false);
    }
}
