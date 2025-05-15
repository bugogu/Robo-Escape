using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float Damage;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Consts.Tags.PLAYER)) 
        {
            other.GetComponent<Player.PlayerController>().GetHit(Damage);
            gameObject.SetActive(false);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        DisableProjectile();
    }
    private void DisableProjectile() => gameObject.SetActive(false);
}