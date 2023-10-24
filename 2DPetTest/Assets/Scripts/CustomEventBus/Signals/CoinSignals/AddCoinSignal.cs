namespace CustomEventBus.Signals
{
    public class AddCoinSignal
    {
        public readonly int Value;

        public AddCoinSignal(int value)
        {
            Value = value;
        }
    }
}