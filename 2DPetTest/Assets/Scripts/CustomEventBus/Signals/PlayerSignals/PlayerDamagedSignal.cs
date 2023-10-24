namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о том что игрок получил урон на Health
    /// </summary>
    public class PlayerDamagedSignal
    {
        public readonly float Health;

        public PlayerDamagedSignal(float health)
        {
            Health = health;
        }
    }
}