using Items;

namespace CustomEventBus.Signals
{
    public class RemoveItemSignal
    {
        public readonly Item Item;
        public readonly int Index;
        public RemoveItemSignal(Item item)
        {
            Item = item;
        }
        public RemoveItemSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

