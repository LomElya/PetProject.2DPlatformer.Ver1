using UnityEngine;


public class Damageable : MonoBehaviour
{
    public float _damageMultiplier = 1f;
    [Range(0, 1)] public float _sensibilityToSelfdamage = 0.5f;
    [Header("Разное")]
    [SerializeField] private Health Health;
    public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
    {
        if (Health)
        {
            var totalDamage = damage;

            // skip the crit multiplier if it's from an explosion
            if (!isExplosionDamage)
            {
                totalDamage *= _damageMultiplier;
            }

            // potentially reduce damages if inflicted by self
            if (Health.gameObject == damageSource)
            {
                totalDamage *= _sensibilityToSelfdamage;
            }

            // apply the damages
            Health.TakeDamage(totalDamage, damageSource);
        }
    }
}
