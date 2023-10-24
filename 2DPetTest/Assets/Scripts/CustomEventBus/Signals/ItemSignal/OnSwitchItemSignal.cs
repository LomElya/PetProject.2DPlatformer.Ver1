using Items;

namespace CustomEventBus.Signals
{
    public class OnSwitchItemSignal
    {
        public readonly Item Item;
        public readonly int Index;
        public OnSwitchItemSignal(Item item)
        {
            Item = item;
        }
        public OnSwitchItemSignal(Item item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}
