using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

namespace Items.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        public List<Item> _inventoryItem = new List<Item>();
        private EventBus _eventBus;

    }
}
