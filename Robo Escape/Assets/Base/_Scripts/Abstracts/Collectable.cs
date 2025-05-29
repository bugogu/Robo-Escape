using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    /// <returns> If the collected item is beneficial for the player. </returns>
    public abstract void Collected();

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Consts.Tags.PLAYER)) Collected();
    }
}