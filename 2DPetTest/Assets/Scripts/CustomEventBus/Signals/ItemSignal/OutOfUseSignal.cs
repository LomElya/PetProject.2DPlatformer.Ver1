using Items;

namespace CustomEventBus.Signals
{
    public class OutOfUseSignal
    {   
        public readonly Item Item;
        public readonly int Index;
        public OutOfUseSignal(Item item)
        {
            Item = item;
        }
        public OutOfUseSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

