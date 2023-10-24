using Items;

namespace CustomEventBus.Signals
{
    public class TryAddItemSignal
    {   
        public readonly Item Item;
        public readonly int Index;
        public TryAddItemSignal(Item item)
        {
            Item = item;
        }
        public TryAddItemSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

