using Items;

namespace CustomEventBus.Signals
{
    public class AddItemInEquippedSlotSignal
    {
        public readonly Item Item;
        public readonly int Index;
        public readonly ItemType TypeItem;
        public AddItemInEquippedSlotSignal(Item item)
        {
            Item = item;
        }
        public AddItemInEquippedSlotSignal(Item item, ItemType typeItem)
        {
            Item = item;
            TypeItem = typeItem;
        }
        public AddItemInEquippedSlotSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

