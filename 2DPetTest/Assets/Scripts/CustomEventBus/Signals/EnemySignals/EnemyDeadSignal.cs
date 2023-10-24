using Enemies;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о смерти врага
    /// </summary>
    public class EnemyDeadSignal
    {
        public readonly Enemy Enemy;
        public EnemyDeadSignal(Enemy enemy)
        {
            Enemy = enemy;
        } 
    }
}