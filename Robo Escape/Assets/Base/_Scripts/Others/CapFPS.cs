using UnityEngine;

public class CapFPS : MonoBehaviour
{
    [SerializeField] private RectTransform _checkMark;
    [SerializeField] private RectTransform _button30;
    [SerializeField] private RectTransform _button60;
    [SerializeField] private GameObject _warningText;

    private int _targetFrameRate;

    private void Start()
    {
        _targetFrameRate = PlayerPrefs.GetInt(Consts.Prefs.CAPFPS, 30);
        Application.targetFrameRate = _targetFrameRate;

        if(_targetFrameRate == 60)
        {
            _checkMark.SetParent(_button60);
            _checkMark.anchoredPosition = Vector2.zero;
            _warningText.SetActive(true);
        }
    }

    public void Button30()
    {
        if(PlayerPrefs.GetInt(Consts.Prefs.CAPFPS, 30) == 30) return;

        Settings.Instance.PlayButtonSound();

        _checkMark.SetParent(_button30);
        _checkMark.anchoredPosition = Vector2.zero;

        PlayerPrefs.SetInt(Consts.Prefs.CAPFPS, 30);
        Application.targetFrameRate = 30;

        _warningText.SetActive(false);
    }

    public void Button60()
    {
        if(PlayerPrefs.GetInt(Consts.Prefs.CAPFPS, 30) == 60) return;

        Settings.Instance.PlayButtonSound();

        _checkMark.SetParent(_button60);
        _checkMark.anchoredPosition = Vector2.zero;

        PlayerPrefs.SetInt(Consts.Prefs.CAPFPS, 60);
        Application.targetFrameRate = 60;

        _warningText.SetActive(true);
    }
}
