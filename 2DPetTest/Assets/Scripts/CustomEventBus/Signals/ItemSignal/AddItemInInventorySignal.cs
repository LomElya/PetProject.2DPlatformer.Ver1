using Items;

namespace CustomEventBus.Signals
{
    public class AddItemInInventorySignal
    {   
        public readonly Item Item;
        public readonly int Index;
        public AddItemInInventorySignal(Item item)
        {
            Item = item;
        }
        public AddItemInInventorySignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

