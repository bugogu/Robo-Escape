using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioClip ElectricSfx, JumpPadSfx, GateClosingSfx;

    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip) 
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
