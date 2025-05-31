using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioClip ElectricSfx, JumpPadSfx, GateClosingSfx, LoseSfx, WinSfx, DigitSfx;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip) 
    {
        if(Settings.Instance.Sound == 0) return;
        
        _audioSource.clip = clip;
        _audioSource?.Play();
    }
}