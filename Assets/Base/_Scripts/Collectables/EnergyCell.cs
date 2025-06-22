using UnityEngine;

public class EnergyCell : Collectable
{
    [SerializeField] private float _replenishAmount = 35f;

    private GameObject _player;
    
    private void Start() =>
        _player = GameObject.FindGameObjectWithTag(Consts.Tags.PLAYER);

    public override void Collected()
    {
        if (Settings.Instance.Sound == 1) _player.GetComponent<AudioSource>()?.Play();

        EnergyBar.Instance.ReplenishEnergy(_replenishAmount, true);
        gameObject.SetActive(false);

        Player.PlayerController.Instance.ParticleReferences.EnergyCellFX.Play();
    }
}