using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform _targetPosition;

    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Consts.Tags.PLAYER);
    }

    public void Teleport()
    {
        CameraShake.Shake();
        _player.transform.position = _targetPosition.position;
        _player.GetComponent<Player.PlayerController>().TeleportFX.SetActive(true);
        Invoke(nameof(StopTeleportFX), 1.5f);
    }

    private void StopTeleportFX() => _player.GetComponent<Player.PlayerController>().TeleportFX.SetActive(false); 
}