using Enemies;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о обнаружении цели врагом
    /// </summary>
    public class EnemyDetectSignal
    {
        public readonly Enemy Enemy;

        public EnemyDetectSignal(Enemy enemy)
        {
            Enemy = enemy;
        } 
    }
}