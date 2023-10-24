using Items;
using UnityEngine;

namespace CustomEventBus.Signals
{
    public class OnUseHealthSignal
    {
        public readonly Item Item;
        public readonly float Value;

        public OnUseHealthSignal(Item item)
        {
            Item = item;
        }
        public OnUseHealthSignal(Item item, float value)
        {
            Item = item;
            Value = value;
        }
    }
}
