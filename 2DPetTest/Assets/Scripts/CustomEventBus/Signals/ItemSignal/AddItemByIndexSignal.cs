using Items;

namespace CustomEventBus.Signals
{
    public class AddItemByIndexSignal
    {
        public readonly Item Item;
        public readonly int Index;
        public AddItemByIndexSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

