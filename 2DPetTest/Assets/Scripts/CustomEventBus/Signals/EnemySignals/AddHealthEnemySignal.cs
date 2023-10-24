using UnityEngine;
using Enemies;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал "Прибавь здоровья врагу"
    /// </summary>
    public class AddHealthEnemySignal
    {
        public readonly float Value;
        public readonly Enemy Enemy;

        public AddHealthEnemySignal(float value)
        {
            Value = value;
        }
        public AddHealthEnemySignal(float value, Enemy enemy)
        {
            Value = value;
            Enemy = enemy;
        }
    }
}