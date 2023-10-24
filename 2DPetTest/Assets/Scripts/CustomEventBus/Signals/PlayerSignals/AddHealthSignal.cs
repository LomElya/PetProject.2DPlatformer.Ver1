using UnityEngine;

namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал "Прибавь здоровья игроку"
    /// </summary>
    public class AddHealthSignal
    {
        public readonly float Value;
        public readonly GameObject _gameObject;

        public AddHealthSignal(float value)
        {
            Value = value;
        }
        public AddHealthSignal(float value, GameObject gameObject)
        {
            Value = value;
            _gameObject = gameObject;
        }
    }
}