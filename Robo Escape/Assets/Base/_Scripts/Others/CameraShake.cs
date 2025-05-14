using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameDesignData _gameDesignData;

    private static CinemachineImpulseSource _shakeSource;
    private static CinemachineImpulseListener _cinemachineImpulseListener;
    private float _duration, _amplitude, _frequency;

    void Start()
    {
        _shakeSource = GetComponent<CinemachineImpulseSource>();
        _cinemachineImpulseListener = GetComponent<CinemachineImpulseListener>();

        _duration = _gameDesignData.CameraShakeDuration;
        _amplitude = _gameDesignData.CameraShakeAmplitude;
        _frequency = _gameDesignData.CameraShakeFrequency;

        _cinemachineImpulseListener.ReactionSettings.AmplitudeGain = _amplitude;
        _cinemachineImpulseListener.ReactionSettings.Duration = _duration;
        _cinemachineImpulseListener.ReactionSettings.FrequencyGain = _frequency;
        
    }

    public static void Shake()
    {
        _shakeSource.GenerateImpulse(); // Kamera sallanır
    }
}