using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Items.Weapons;
using Items;

namespace UI.Dialogs
{
    public abstract class MainInventory : MonoBehaviour
    {
        [Header("Основное")]
        [SerializeField] protected RectTransform _itemParent;
        [SerializeField] protected RectTransform _draggableParant;
        [SerializeField] protected InventorySlot _inventorySlot;
        [SerializeField] protected int _startCountSlots = 7;
        protected List<InventorySlot> _slots = new List<InventorySlot>();
        protected PlayersItemManager _playersItemManager;
        //protected InventorySlot[] _slots;
        protected EventBus _eventBus;
        protected virtual void Awake()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<GameStartedSignal>(GameStarted);

            _eventBus.Subscribe<OutOfUseSignal>(RemoveItem);
            _eventBus.Subscribe<OnSwitchItemSignal>(OnSwitchedItem);

            AddSlot(_slots);

            _playersItemManager = FindObjectOfType<PlayersItemManager>();

        }
        private void GameStarted(GameStartedSignal signal)
        {
            Debug.Log("Старт игры");
        }

        protected virtual void AddSlot(List<InventorySlot> slots)
        {
            for (int i = 0; i < _startCountSlots; i++)
            {
                var inventorySlot = Instantiate(_inventorySlot, _itemParent);
                inventorySlot.Init(_draggableParant);
                inventorySlot.SetIndex(i);
                slots.Add(inventorySlot);
            }
        }
        protected virtual bool AddItem(Item item, int index)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i]._item == item)
                {
                    if (_slots[i].TryAddAmount(item, 1))
                    {
                        return true;
                    }
                }
                if (_slots[i]._item == null)
                {
                    _eventBus.Invoke(new TryAddItemSignal(item));
                    _slots[i].AddItem(item, i);
                    return true;
                }
            }
            return false;
        }
        protected virtual void RemoveItem(OutOfUseSignal signal)
        {
            Item item = signal.Item;
            int index = signal.Index;

            for (int i = 0; i < _slots.Count; i++)
            {
                if (item == _slots[i]._item || index == _slots[i]._itemCounterIndex)
                {
                    _slots[i].RemoveItem(item);
                    return;
                }
            }
        }
        protected virtual void OnSwitchedItem(OnSwitchItemSignal signal)
        {

        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GameStartedSignal>(GameStarted);
            _eventBus.Unsubscribe<OutOfUseSignal>(RemoveItem);
            _eventBus.Unsubscribe<OnSwitchItemSignal>(OnSwitchedItem);
        }
    }
}