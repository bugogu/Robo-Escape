using UnityEngine;

public class JumpPadArea : MonoBehaviour
{
    public Transform Target;
    public bool InArea = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Consts.Tags.PLAYER)) return;

        Check(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(Consts.Tags.PLAYER)) return;

        Check(false);
    }

    private void Check(bool status)
    {
        InArea = status;
        Target.gameObject.SetActive(status);
    }
}