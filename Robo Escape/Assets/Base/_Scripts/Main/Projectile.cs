using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Consts.Tags.PLAYER)) 
        {
            other.GetComponent<PlayerController>().GetHit(damage);
            gameObject.SetActive(false);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        DisableProjectile();
    }
    private void DisableProjectile() => gameObject.SetActive(false);
}
