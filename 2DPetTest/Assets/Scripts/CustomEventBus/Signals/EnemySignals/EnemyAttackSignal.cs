using Enemies;
using UnityEngine;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о смерти врага
    /// </summary>
    public class EnemyAttackSignal
    {
        public readonly Enemy Enemy;
        public readonly GameObject Hurt;
        public EnemyAttackSignal(Enemy enemy, GameObject hurt)
        {
            Enemy = enemy;
            Hurt = hurt;
        }
    }
}