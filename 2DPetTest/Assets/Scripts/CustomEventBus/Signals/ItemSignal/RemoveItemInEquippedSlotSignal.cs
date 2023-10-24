using Items;

namespace CustomEventBus.Signals
{
    public class RemoveItemInEquippedSlotSignal
    {
        public readonly Item Item;
        public readonly int Index;
        public readonly ItemType TypeItem;
        public RemoveItemInEquippedSlotSignal(Item item)
        {
            Item = item;
        }
        public RemoveItemInEquippedSlotSignal(Item item, ItemType typeItem)
        {
            Item = item;
            TypeItem = typeItem;
        }
        public RemoveItemInEquippedSlotSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

