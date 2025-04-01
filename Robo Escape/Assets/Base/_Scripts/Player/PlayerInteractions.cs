using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerController _playerController;

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ICollectable collectable)) 
        {
            var fxPlay = collectable.Collect() ? _playerController._energyCellFX : _playerController._drainCellFX;
            fxPlay.Play();
        }
        if(other.CompareTag(Consts.Tags.MAGNETIC_AREA)) _playerMovement._isMagnetized = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(Consts.Tags.MAGNETIC_AREA)) _playerMovement._isMagnetized = false;
    }
}
