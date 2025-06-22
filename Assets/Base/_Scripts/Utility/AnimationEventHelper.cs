using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    public void PlaySFX()
    { 
        SoundManager.Instance.PlaySFX(SoundManager.Instance.GateClosingSfx);
    }
}
