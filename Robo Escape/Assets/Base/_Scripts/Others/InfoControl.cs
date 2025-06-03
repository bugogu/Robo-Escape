using UnityEngine;

public class InfoControl : MonoBehaviour
{
    private int _activeIndex = 0;
    private GameObject[] _infos;

    private void Start()
    {
        _infos = new GameObject[transform.childCount];

        for (int i = 0; i < _infos.Length; i++)
            _infos[i] = transform.GetChild(i).gameObject;

        _infos[_activeIndex].SetActive(true);
    }

    public void NextInfo()
    {
        Settings.Instance.PlayButtonSound();

        _infos[_activeIndex].SetActive(false);
        _activeIndex = (_activeIndex + 1) % _infos.Length;

        _infos[_activeIndex].SetActive(true);
    }
    
    public void PreviousInfo()
    {
        Settings.Instance.PlayButtonSound();
        
        _infos[_activeIndex].SetActive(false);
        _activeIndex = (_activeIndex - 1 + _infos.Length) % _infos.Length;

        _infos[_activeIndex].SetActive(true);
    }
}
