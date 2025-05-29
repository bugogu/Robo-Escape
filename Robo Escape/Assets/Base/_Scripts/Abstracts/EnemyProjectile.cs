using UnityEngine;

public abstract class EnemyProjectile : MonoBehaviour
{
    protected abstract void GiveDamage(Collider player);

    protected virtual void DestroyProjectile() => gameObject.SetActive(false);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.Tags.PLAYER)) GiveDamage(other);
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle")) DestroyProjectile();
    }
}