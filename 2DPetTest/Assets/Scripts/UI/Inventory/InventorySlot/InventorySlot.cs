using Items;
using UnityEngine;
using UnityEngine.UI;
using Items.Weapons;
using Platformer.Mechanics;
using Interactables;
using CustomEventBus;


namespace UI.Dialogs
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] public DragAndDropItem _rootSlot;
        [SerializeField] public Button _buttonSlot;
        [Header("Предмет:")]
        [SerializeField] public Item _item;
        [SerializeField] public Text _nameItem;
        [SerializeField] public Image _itemIcon;
        [SerializeField] protected Image _amountImage;
        [SerializeField] public Text _amountText;
        public Item Item => _item;
        public bool _isOpenInfo { get; set; }
        public int _itemCounterIndex { get; set; }
        public int _amount { get; set; }
        public bool _isEmptySlot { get; set; }
        protected EventBus _eventBus;

        public virtual void Awake()
        {
            _isEmptySlot = true;
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _amount = 1;
        }
        public virtual void Init(RectTransform inventoryParent)
        {
            _rootSlot.Init(inventoryParent, _eventBus);
        }
        public virtual void AddItem(Item item, int index)
        {
            TryItemAdd(item);
            //Перенести
            /* _itemCounterIndex = index;
            Debug.Log(index); */
        }
        public virtual void AddItem(Item item)
        {
            TryItemAdd(item);
        }
        public virtual bool TryItemAdd(Item item)
        {
            if (item != null)
            {
                _buttonSlot.onClick.AddListener(OpenInfoSlot);

                _isEmptySlot = false;

                _item = item;
                if (GetMaxAmountInSlot(item) <= 1)

                    _amountImage.gameObject.SetActive(false);
                else
                {
                    _amountImage.gameObject.SetActive(true);
                    _amountText.text = _amount.ToString();
                }

                _nameItem.text = _item.ItemData.NameItem;
                _itemIcon.sprite = _item.ItemData.IconItem;

                _rootSlot.gameObject.SetActive(true);

                return true;
            }

            return false;
        }
        public bool TryAddAmount(Item item, int amount)
        {
            if (_amount + amount <= GetMaxAmountInSlot(item))
            {
                _amount += amount;
                _amountText.text = _amount.ToString();
                return true;
            }
            return false;
        }
        public bool TryUseItem(Item item)
        {
            if (_amount-- > 0)
            {
                _amount--;
                _amountText.text = _amount.ToString();
                Debug.Log("Кол-во " + _amount);
                return true;
            }
            return false;
        }
        public virtual bool TryRemoveItem(Item item)
        {
            _item = null;

            _isEmptySlot = true;

            _nameItem.text = "";
            _itemIcon.sprite = null;
            _rootSlot.gameObject.SetActive(false);
            _amount = 1;
            _amountText.text = _amount.ToString();

            return true;
        }
        public void OpenInfoSlot()
        {
            if (_isOpenInfo)
                return;

            if (_item)
            {
                var infoItem = DialogsManager.ShowDialog<InfoItem>();

                infoItem.Init(this);
                ItemType itemType = _item.ItemData.TypeItem;

                if (itemType == ItemType.Weapon || itemType == ItemType.Armor)
                    infoItem.OpenInfo(_item, "Эпипировать");
                else
                    infoItem.OpenInfo(_item, "Использовать");

                _isOpenInfo = true;
            }
        }

        public virtual void RemoveItem(Item item)
        {

            TryRemoveItem(item);
        }
        public virtual void DropItem(Item item, int amount)
        {
            PlayerController playerController = FindFirstObjectByType<PlayerController>();

            Camera camera = Camera.main;
            Vector2 pos = camera.ScreenToWorldPoint(Input.mousePosition);
            //pos = camera.WorldToScreenPoint(pos);

            ItemInteractable prefabInteractableItem = item.ItemData.PrefabInteractableItem;
            // Выброс объектов из инвентаря - Спавним префаб обекта перед персонажем
            ItemInteractable itemObject = Instantiate(prefabInteractableItem, pos, Quaternion.identity);
            // Устанавливаем количество объектов такое какое было в слоте
            itemObject.ItemPrefab._amountCurrent = amount;

            TryRemoveItem(item);
        }
        public virtual void SelectItem()
        {
            _buttonSlot.colors = ChangeTransparency(_buttonSlot.colors, 1f);
        }
        public virtual void NotSelectItem()
        {
            _buttonSlot.colors = ChangeTransparency(_buttonSlot.colors, 0.4f);
        }

        private ColorBlock ChangeTransparency(ColorBlock color, float transparency)
        {
            Color normalColor = color.normalColor;
            normalColor.a = transparency;

            color.normalColor = normalColor;

            return color;
        }
        public int GetMaxAmountInSlot(Item item)
        {
            return item.ItemData.MaximumAmountInSlot;
        }
        public void SetIndex(int index)
        {
            _itemCounterIndex = index;
        }
        public int GetIndexSlot()
        {
            return _itemCounterIndex;
        }
    }

}
