using UnityEngine;

public class Chipset : MonoBehaviour, ICollectable
{
    private GameObject _player;
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Consts.Tags.PLAYER);
    }

    public bool Collect()
    {
        if(Settings.Instance.Sound == 1) _player.GetComponent<AudioSource>()?.Play();
        LevelManager.Instance.CollectChipset();
        gameObject.SetActive(false);
        return true;
    }
}
