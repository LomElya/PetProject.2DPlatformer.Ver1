using Items;
using UnityEngine;
using CustomEventBus.Signals;

namespace UI.Dialogs
{
    public class InventorySlotEquipped : InventorySlot
    {

        [Header("Стандартная картинка:")]
        [SerializeField] private Sprite _imageStandart;
        [Header("Тип предмета для слота:")]
        [SerializeField] private ItemType _itemType;
        [Header("Остальное:")]
        [SerializeField] private RectTransform _inventoryParent;
        public ItemType ItemType => _itemType;
        public override void Awake()
        {
            base.Awake();
            _itemIcon.sprite = _imageStandart;
        }
        private void Start()
        {
            //Init(_inventoryParent);
        }
        public void AddItemInActiveSlot(Item item, int index)
        {
            _item = item;
            //_nameItem.text = _item.ItemData.NameItem;
            _itemIcon.sprite = _item.ItemData.IconItem;

            _itemCounterIndex = index;

            _rootSlot.gameObject.SetActive(true);
        }
        public override void AddItem(Item item)
        {
            TryItemAdd(item);
            _eventBus.Invoke(new AddItemInEquippedSlotSignal(_item));
        }
        public override void RemoveItem(Item item)
        {
            if (_item != null)
            {
                _eventBus.Invoke(new RemoveItemInEquippedSlotSignal(_item));
            }
            TryRemoveItem(item);
        }

        public override bool TryItemAdd(Item item)
        {
            if (item != null)
            {
                _buttonSlot.onClick.AddListener(OpenInfoSlot);


                _isEmptySlot = false;

                _item = item;
                _itemIcon.sprite = _item.ItemData.IconItem;


                return true;
            }

            return false;
        }
        public override bool TryRemoveItem(Item item)
        {
            _isEmptySlot = true;



            _item = null;
            _itemIcon.sprite = _imageStandart;

            return true;

        }
    }

}
