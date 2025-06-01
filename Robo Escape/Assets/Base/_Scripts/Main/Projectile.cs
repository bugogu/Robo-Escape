using UnityEngine;

public class Projectile : EnemyProjectile
{
    [HideInInspector] public float Damage;
    [HideInInspector] public float KnockbackForce;

    protected override void GiveDamage(Collider player)
    {
        if (KnockbackForce.Equals(0)) KnockbackForce = 300f;

        player.GetComponent<Player.PlayerController>().GetHit(Damage);
        player.GetComponent<Rigidbody>().AddForce(transform.forward * KnockbackForce, ForceMode.Impulse);
        gameObject.SetActive(false);
    }
}