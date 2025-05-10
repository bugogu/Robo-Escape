using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameDesignData _gameDesignData;

    private static CinemachineImpulseSource _shakeSource;

    private static CinemachineImpulseListener _cinemachineImpulseListener;

    private float _duration;
    private float _amplitude;
    private float _frequency;

    void Start()
    {
        _shakeSource = GetComponent<CinemachineImpulseSource>();
        _cinemachineImpulseListener = GetComponent<CinemachineImpulseListener>();

        _duration = _gameDesignData.cameraShakeDuration;
        _amplitude = _gameDesignData.cameraShakeAmplitude;
        _frequency = _gameDesignData.cameraShakeFrequency;

        _cinemachineImpulseListener.ReactionSettings.AmplitudeGain = _amplitude;
        _cinemachineImpulseListener.ReactionSettings.Duration = _duration;
        _cinemachineImpulseListener.ReactionSettings.FrequencyGain = _frequency;
        
    }

    public static void Shake()
    {
        _shakeSource.GenerateImpulse(); // Kamera sallanır
    }
}
