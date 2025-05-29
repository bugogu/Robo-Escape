using UnityEngine;

public class Chipset : Collectable
{
    private GameObject _player;

    private void Start() =>
        _player = GameObject.FindGameObjectWithTag(Consts.Tags.PLAYER);

    public override void Collected()
    {
        if (Settings.Instance.Sound == 1) _player.GetComponent<AudioSource>()?.Play();
        LevelManager.Instance.CollectChipset();
        gameObject.SetActive(false);

        Player.PlayerController.Instance.ParticleReferences.EnergyCellFX.Play();
    }
}