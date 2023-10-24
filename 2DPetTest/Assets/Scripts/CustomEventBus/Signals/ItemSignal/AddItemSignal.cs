using Items;

namespace CustomEventBus.Signals
{
    public class AddItemSignal
    {   
        public readonly Item Item;
        public readonly int Index;
        public AddItemSignal(Item item)
        {
            Item = item;
        }
        public AddItemSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

