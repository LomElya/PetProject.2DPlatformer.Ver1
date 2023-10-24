using UnityEngine;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о том, что здоровье изменилось
    /// </summary>
    public class HealthChangedSignal
    {
        public readonly float MaxHealth;
        public readonly float Health;

        public readonly float Damage;
        public readonly bool IsDamage;
        public readonly GameObject DamageSource;
        public readonly GameObject Owner;

        public HealthChangedSignal(float health, float maxHealth, GameObject owner)
        {
            Health = health;
            MaxHealth = maxHealth;
            Owner = owner;
        }
        public HealthChangedSignal(float damage, GameObject damageSource, bool isDamage)
        {
            Damage = damage;
            DamageSource = damageSource;
            IsDamage = isDamage;
        }

    }
}