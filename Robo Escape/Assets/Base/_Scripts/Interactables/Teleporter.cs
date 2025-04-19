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
        _player.transform.position = _targetPosition.position;
    }
}
