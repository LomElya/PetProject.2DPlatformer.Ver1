using System.Collections.Generic;
using Items;
using UnityEngine;
using Items.Weapons;
using CustomEventBus.Signals;
using UnityEngine.UI;
using CustomEventBus;

namespace UI.Dialogs
{
    public class InventoryWindow : MainInventory
    {
        [Header("Инвентарь")]
        [SerializeField] GameObject _inventory;
        [SerializeField] Button _exitButton;
        [Header("Слоты брони")]
        [SerializeField] InventorySlotEquipped _slotHeadArmor;
        [SerializeField] InventorySlotEquipped _slotBodyArmor;
        [SerializeField] InventorySlotEquipped _slotPantsArmor;
        [SerializeField] InventorySlotEquipped _slotShoesArmor;
        [Header("Слоты оружия и др.")]
        [SerializeField] InventorySlotEquipped _slotWeapon;
        [SerializeField] InventorySlotEquipped _slotBag;
        [SerializeField] InventorySlotEquipped _slotShield;
        [Header("Статы в инвенторе")]
        [SerializeField] Text _maxHealthText;
        [SerializeField] Text _currentHealthText;
        [SerializeField] Text _damageText;
        [SerializeField] Text _armorText;

        protected override void Awake()
        {
            base.Awake();

            Item activeItem = _playersItemManager.GetActiveItem();

            _exitButton.onClick.AddListener(OnExitButtonClick);

            _eventBus.Subscribe<OpenInventorySignal>(x => { _inventory.gameObject.SetActive(true); });
            _eventBus.Subscribe<GameUnPauseSignal>(x => { _inventory.gameObject.SetActive(false); });
            _eventBus.Subscribe<HealthChangedSignal>(DisplayHealth);
            _eventBus.Subscribe<AddItemInEquippedSlotSignal>(AddItemInEquippedSlot);
            _eventBus.Subscribe<RemoveItemInEquippedSlotSignal>(RemoveItemInEquippedSlot);
            _eventBus.Subscribe<AddItemInInventorySignal>(AddItemInInventory);
        }
        private void Start()
        {
            Health healthPlayer = _playersItemManager.GetComponent<Health>();
            DisplayHealth(healthPlayer.MaxHealth, healthPlayer.CurrentHealth);
            _armorText.text = 0.ToString();
        }

        private void DisplayHealth(float maxHealth, float currentHealth)
        {
            _maxHealthText.text = "/ " + maxHealth.ToString();
            _currentHealthText.text = currentHealth.ToString();
        }
        public void DisplayDamage(Item item)
        {
            if (item != null && item == item.GetComponent<Weapon>())
            {
                Weapon weapon = item.GetComponent<Weapon>();
                _damageText.text = weapon.Damage.ToString();
            }
            else
                _damageText.text = 0.ToString();
        }
        protected override void OnSwitchedItem(OnSwitchItemSignal signal)
        {
            //DisplayDamage(signal.Item);
        }

        public void DisplayHealth(HealthChangedSignal signal)
        {
            if (signal.Owner.GetComponent<Player>())
            {
                DisplayHealth(signal.MaxHealth, signal.Health);
            }
        }

        private void OnExitButtonClick()
        {
            _eventBus.Invoke(new GameUnPauseSignal());
            //Hide();
        }
        private void AddItemInEquippedSlot(AddItemInEquippedSlotSignal signal)
        {
            DisplayDamage(signal.Item);

            AddItem(signal.Item, signal.Index);
        }

        protected override bool AddItem(Item item, int index)
        {
            if (!base.AddItem(item, index))
            {
                Debug.Log("Инвентарь полный");
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
                inventorySlot.SetIndex(i + 7);
                slots.Add(inventorySlot);
            }
        }
        private void AddItemInInventory(AddItemInInventorySignal signal)
        {
            AddItem(signal.Item, signal.Index);
        }
        private void RemoveItemInEquippedSlot(RemoveItemInEquippedSlotSignal signal)
        {
            ItemType itemType = signal.Item.ItemData.TypeItem;

            switch (itemType)
            {
                case ItemType.Weapon:
                    DisplayDamage(null);
                    break;
            }
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<HealthChangedSignal>(DisplayHealth);
            _eventBus.Unsubscribe<AddItemInEquippedSlotSignal>(AddItemInEquippedSlot);
            _eventBus.Unsubscribe<RemoveItemInEquippedSlotSignal>(RemoveItemInEquippedSlot);
            _eventBus.Unsubscribe<AddItemInInventorySignal>(AddItemInInventory);
        }
    }

}


