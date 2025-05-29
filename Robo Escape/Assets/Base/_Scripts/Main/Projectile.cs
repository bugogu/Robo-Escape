using UnityEngine;

public class Projectile : EnemyProjectile
{
    [HideInInspector] public float Damage;
    
    protected override void GiveDamage(Collider player)
    {
        player.GetComponent<Player.PlayerController>().GetHit(Damage);
        gameObject.SetActive(false);
    }
}