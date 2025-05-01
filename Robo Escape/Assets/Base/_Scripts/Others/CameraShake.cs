using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameDesignData _gameDesignData;

    private static CinemachineImpulseSource _shakeSource;

    private float _duration;
    private float _amplitude;
    private float _frequency;

    void Start()
    {
        _shakeSource = GetComponent<CinemachineImpulseSource>();

        _duration = _gameDesignData.cameraShakeDuration;
        _amplitude = _gameDesignData.cameraShakeAmplitude;
        _frequency = _gameDesignData.cameraShakeFrequency;
    }

    public static void Shake()
    {
        _shakeSource.GenerateImpulse(); // Kamera sallanır
    }
}
