using UnityEngine;

public class JumpPadArea : MonoBehaviour
{
    public Transform target;
    public bool inArea = false;

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(Consts.Tags.PLAYER)) return;

        inArea = true;
        target.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(Consts.Tags.PLAYER)) return;

        inArea = false;
        target.gameObject.SetActive(false);
    }
}
