using UnityEngine;

public class JumpPadArea : MonoBehaviour
{
    public Transform Target;
    public bool InArea = false;

    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(Consts.Tags.PLAYER)) return;

        InArea = true;
        Target.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(Consts.Tags.PLAYER)) return;

        InArea = false;
        Target.gameObject.SetActive(false);
    }
}