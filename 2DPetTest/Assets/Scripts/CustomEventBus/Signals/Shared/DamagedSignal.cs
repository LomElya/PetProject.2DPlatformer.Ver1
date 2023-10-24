using UnityEngine;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о получении урона
    /// </summary>
    public class DamageSignal
    {
        public readonly float Damage;
        public readonly GameObject DamageSource;
        public readonly GameObject Hurt;
        

        public DamageSignal(float damage, GameObject damageSource)
        {
            Damage = damage;
            DamageSource = damageSource;
        }
        public DamageSignal(float damage, GameObject damageSource, GameObject hurt)
        {
            Damage = damage;
            DamageSource = damageSource;
            Hurt = hurt;
        }
    }
}