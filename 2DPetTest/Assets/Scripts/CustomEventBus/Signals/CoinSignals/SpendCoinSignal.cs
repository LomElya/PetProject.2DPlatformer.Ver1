namespace CustomEventBus.Signals
{
    public class SpendCoinSignal
    {
        public readonly int Value;

        public SpendCoinSignal(int value)
        {
            Value = value;
        }
    }
}