using Items;

namespace CustomEventBus.Signals
{
    public class RemoveItemByIndexSignal
    {
        public readonly Item Item;
        public readonly int Index;
        public RemoveItemByIndexSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

