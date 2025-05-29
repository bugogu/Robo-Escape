using UnityEngine;

public class DrainCell : Collectable
{
    [SerializeField] private float _consumeAmount = 20f;

    public override void Collected()
    {
        if (Settings.Instance.Haptic == 1) Handheld.Vibrate();
        if (Settings.Instance.Sound == 1) SoundManager.Instance.PlaySFX(SoundManager.Instance.ElectricSfx);
        CameraShake.Shake();
        EnergyBar.Instance.ConsumeEnergy(_consumeAmount, true);
        gameObject.SetActive(false);

        Player.PlayerController.Instance.ParticleReferences.DrainCellFX.Play();
    }
}