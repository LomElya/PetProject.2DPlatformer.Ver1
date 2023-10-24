using UnityEngine;
using Enemies;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о том, что здоровье врага изменилось
    /// </summary>
    public class HealthEnemyChangedSignal
    {
        public readonly float MaxHealth;
        public readonly float Health;
        public readonly Enemy Enemy;
        public HealthEnemyChangedSignal(float health, float maxHealth)
        {
            Health = health;
            MaxHealth = maxHealth;
        }
        public HealthEnemyChangedSignal(float health, float maxHealth, Enemy enemy)
        {
            Health = health;
            MaxHealth = maxHealth;
            Enemy = enemy;
        }
        
    }
}