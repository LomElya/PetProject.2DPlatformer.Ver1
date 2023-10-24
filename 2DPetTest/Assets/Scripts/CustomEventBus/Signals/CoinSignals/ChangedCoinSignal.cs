namespace CustomEventBus.Signals
{
    public class ChangedCoinSignal
    {
        public readonly int Value;

        public ChangedCoinSignal(int value)
        {
            Value = value;
        }
    }
}