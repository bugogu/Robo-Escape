using UnityEngine;

public class Projectile : EnemyProjectile
{
    [HideInInspector] public float Damage;
    [HideInInspector] public float KnockbackForce;

    private Transform _hitEffect;

    protected override void GiveDamage(Collider player)
    {
        if (KnockbackForce.Equals(0)) KnockbackForce = 300f;

        HitEffect();

        player.GetComponent<Player.PlayerController>().GetHit(Damage);
        player.GetComponent<Rigidbody>().AddForce(transform.forward * KnockbackForce, ForceMode.Impulse);
        gameObject.SetActive(false);
    }

    protected override void DestroyProjectile(bool instantDestroy = false)
    {
        HitEffect();

        gameObject.SetActive(false);
    }

    private void HitEffect()
    {
        _hitEffect = transform.GetChild(0);

        _hitEffect.GetChild(0).gameObject.SetActive(true);
        _hitEffect.GetChild(0).parent = null;

        Invoke(nameof(CloseHitEffect), 2f);
    }
    
    private void CloseHitEffect() => _hitEffect.GetChild(0).gameObject.SetActive(false);
}