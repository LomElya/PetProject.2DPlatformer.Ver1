using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Items.Weapons;
using Items;
using System.Collections.Generic;

namespace UI.Dialogs
{
    public class InventoryPanel : MainInventory
    {
        protected override void Awake()
        {
            base.Awake();
            Item activeItem = _playersItemManager.GetActiveItem();
            if (activeItem)
            {
                AddItem(activeItem, _playersItemManager.ActiveItemIndex);
                ChangeWeapon(activeItem, _playersItemManager.ActiveItemIndex);
            }

            _playersItemManager = FindObjectOfType<PlayersItemManager>();
            _eventBus.Subscribe<AddItemSignal>(AddNewItem);
        }
        protected override void OnSwitchedItem(OnSwitchItemSignal signal)
        {
            ChangeWeapon(signal.Item, signal.Index);
        }
        private void ChangeWeapon(Item item, int index)
        {
            Item activeItem = _playersItemManager.GetActiveItem();
            if (index + 1 < _slots.Count)
            {
                foreach (var slot in _slots)
                    slot.NotSelectItem();


                _slots[index].SelectItem();
            }

        }
        protected void AddNewItem(AddItemSignal signal)
        {
            Item newItem = signal.Item;
            int index = signal.Index;

            AddItem(newItem, index);
        }
        protected override bool AddItem(Item item, int index)
        {
            if (!base.AddItem(item, index))
            {
                _eventBus.Invoke(new AddItemInInventorySignal(item, -1));
                return false;
            }

            return true;
        }
        protected override void AddSlot(List<InventorySlot> slots)
        {
            for (int i = 0; i < _startCountSlots; i++)
            {
                var inventorySlot = Instantiate(_inventorySlot, _itemParent);
                inventorySlot.Init(_draggableParant);
                inventorySlot.SetIndex(i);
                slots.Add(inventorySlot);
            }
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<AddItemSignal>(AddNewItem);
        }
    }

}
