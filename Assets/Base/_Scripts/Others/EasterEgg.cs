using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    [SerializeField] private ParticleSystem _confetti;

    void Start()
    {
        if(PlayerPrefs.GetInt(Consts.Prefs.PROTOCOLCOUNT, 0) >= 100)
            gameObject.SetActive(false);
    }

    private void OnMouseDown()=>
        Clicked();

    public void Clicked()
    {
        _confetti.Play();
        GameManager.Instance.ProtocolCount +=1000;
        gameObject.SetActive(false);
    }
}