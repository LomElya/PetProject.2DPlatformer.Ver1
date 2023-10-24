using CustomEventBus;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CustomEventBus.Signals;

namespace UI.Dialogs
{
    public class InfoItem : DialogCore, IDragHandler, IBeginDragHandler
    {
        [SerializeField] private Image _iconItem;
        [SerializeField] private Text _nameItem;
        [SerializeField] private Text _descriptionItem;
        [Header("Кнопки")]
        [SerializeField] private Button _buttonExit;
        [SerializeField] private Button _buttonUseItem;
        [SerializeField] private Text _buttonTextUse;
        [SerializeField] private Button _buttonCancel;
        private Item _item;

        private InventorySlot _mainInventorySlot;
        private EventBus _eventBus;

        protected override void Awake()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            base.Awake();

            _buttonExit.onClick.AddListener(() => { Hide(); _mainInventorySlot._isOpenInfo = false; });
            _buttonUseItem.onClick.AddListener(() => { Hide(); _mainInventorySlot._isOpenInfo = false; });
            _buttonCancel.onClick.AddListener(() => { Hide(); _mainInventorySlot._isOpenInfo = false; });
        }
        public void OpenInfo(Item item, string textUse)
        {
            _item = item;

            _nameItem.text = item.ItemData.NameItem;
            _iconItem.sprite = item.ItemData.IconItem;
            _descriptionItem.text = item.GetDescription();

            _buttonTextUse.text = textUse;
            _buttonUseItem.onClick.AddListener(UseItem);
        }
        public void Init(InventorySlot inventorySlot)
        {
            _mainInventorySlot = inventorySlot;
        }
        private void UseItem()
        {
            ItemType itemType = _item.ItemData.TypeItem;

            switch (itemType)
            {
                case ItemType.Flask:

                    _item.TryUse();
                    _mainInventorySlot.RemoveItem(_item);

                    break;

                case ItemType.Weapon:

                    _eventBus.Invoke(new AddItemInEquippedSlotSignal(_item, ItemType.Weapon));
                    _mainInventorySlot.RemoveItem(_item);

                    break;
            }

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.parent = _draggingParent;
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }
    }
}
