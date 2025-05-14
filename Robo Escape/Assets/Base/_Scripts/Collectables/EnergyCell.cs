using UnityEngine;

public class EnergyCell : MonoBehaviour, ICollectable
{
    [SerializeField] private float _replenishAmount = 35f;

    private GameObject _player;
    
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Consts.Tags.PLAYER);
    }

    public bool Collect()
    {
        if(Settings.Instance.Sound == 1) _player.GetComponent<AudioSource>()?.Play();
        
        EnergyBar.Instance.ReplenishEnergy(_replenishAmount , true);
        gameObject.SetActive(false);
        return true;
    }
}