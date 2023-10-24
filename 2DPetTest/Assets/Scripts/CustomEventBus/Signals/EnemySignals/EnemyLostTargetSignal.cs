using Enemies;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о потери цели врагом
    /// </summary>
    public class EnemyLostTargetSignal
    {
        public readonly Enemy Enemy;

        public EnemyLostTargetSignal(Enemy enemy)
        {
            Enemy = enemy;
        } 
    }
}