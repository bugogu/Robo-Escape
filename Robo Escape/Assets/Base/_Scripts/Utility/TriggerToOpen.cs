using UnityEngine;

public class TriggerToOpen : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToOpen;

    void Start()
    {
        foreach (GameObject obj in _objectsToOpen)
            obj.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag != Consts.Tags.PLAYER) return;

        foreach (GameObject obj in _objectsToOpen)
            obj.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag != Consts.Tags.PLAYER) return;

        foreach (GameObject obj in _objectsToOpen)
            obj.SetActive(false);
    }
}
