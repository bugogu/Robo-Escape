using UnityEngine;

public class DrainCell : MonoBehaviour, ICollectable
{
    [SerializeField] private float _consumeAmount = 35f;
    public bool Collect()
    {
        if(Settings.Instance.Haptic == 1) Handheld.Vibrate(); 
        if(Settings.Instance.Sound == 1) SoundManager.Instance.PlaySFX(SoundManager.Instance.electricSfx);
        CameraShake.Shake();
        EnergyBar.Instance.ConsumeEnergy(_consumeAmount, true);
        gameObject.SetActive(false);
        return false;
    }
}
