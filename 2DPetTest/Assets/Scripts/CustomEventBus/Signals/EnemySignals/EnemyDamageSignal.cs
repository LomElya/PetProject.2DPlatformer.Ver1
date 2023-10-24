using UnityEngine;
using Enemies;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о том что враг получил урон на Health
    /// </summary>
    public class EnemyDamageSignal
    {
        public readonly float Health;
        public readonly float Damage;
        public readonly Enemy Enemy; 
        public readonly GameObject DamageSource;

       
        public EnemyDamageSignal(Enemy enemy, float damage, GameObject damageSource)
        {
            Enemy = enemy;
            Damage = damage;
            DamageSource = damageSource;
        }
        public EnemyDamageSignal(float health, Enemy enemy)
        {
            Health = health;
            Enemy = enemy;
        }
        public EnemyDamageSignal(float health, GameObject damageSource)
        {
            Health = health;
            DamageSource = damageSource;
        }
    }
}