using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;
using CustomEventBus;
using CustomEventBus.Signals;

namespace Items.Weapons
{
    public class PlayersItemManager : MonoBehaviour
    {
        public enum ItemSwitchState
        {
            Up,
            Down,
            PutDownPrevious,
            PutUpNew,
        }
        [SerializeField] private List<Item> _startingItem = new List<Item>();
        [SerializeField] private Transform _itemParentSocket;
        //Задержка перед переключением предмета
        [SerializeField] private float _itemSwitchDelay = 1f;
        [SerializeField] private LayerMask _itemLayer;
        [Header("Разное")]
        [SerializeField] private PlayerInputHandler _inputHandler;

        private Item[] _itemSlots = new Item[21];
        private float _timeStartedItemSwitch;
        private int _itemSwitchNewItemIndex;

        private ItemSwitchState _itemSwitchState;
        private EventBus _eventBus;

        public int ActiveItemIndex { get; private set; }

        /// Init в Player
        public void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            ActiveItemIndex = -1;
            _itemSwitchState = ItemSwitchState.Down;

            _eventBus.Subscribe<OnSwitchItemSignal>(OnSwitchedItem);
            _eventBus.Subscribe<TryAddItemSignal>(TryAddItem);
            _eventBus.Subscribe<RemoveItemByIndexSignal>(RemoveItemByIndex);
            _eventBus.Subscribe<AddItemByIndexSignal>(AddItemByIndex);

            ///Добавить стартовый предмет
            foreach (var item in _startingItem)
            {
                if (item == null)
                    continue;
                _eventBus.Invoke(new AddItemSignal(item, -1));
                //AddItem(item);
            }

            SwitchItem(true);
        }

        private void Update()
        {
            /// Использование/атака
            Item activeItem = GetActiveItem();
            if (activeItem != null && _itemSwitchState == ItemSwitchState.Up)
            {
                bool hasUse = activeItem.HandleUseInputs(
                    _inputHandler.GetUseInputDown(),
                    _inputHandler.GetUseInputHeld(),
                    _inputHandler.GetUseInputReleased());

                // Сопротивление отдачи
                if (hasUse)
                {

                    //Debug.Log("Отдача");
                }
            }
            //Переключение оружия
            if (_itemSwitchState == ItemSwitchState.Up || _itemSwitchState == ItemSwitchState.Down)
            {
                int switchItemInput = _inputHandler.GetSwitchItemInput();

                if (switchItemInput != 0)
                {
                    bool switchUp = switchItemInput > 0;
                    SwitchItem(switchUp);
                }
                else
                {
                    switchItemInput = _inputHandler.GetSelectItemInput();

                    if (switchItemInput != 0)
                    {
                        if (GetItemAtSlotIndex(switchItemInput - 1) != null)
                            SwitchToItemIndex(switchItemInput - 1);
                    }
                }

            }
        }
        private void LateUpdate()
        {
            UpdateItemSwitching();
        }
        public void SwitchItem(bool ascendingOrder)
        {
            int newItemIndex = -1;
            int closestSlotDistance = _itemSlots.Length;
            for (int i = 0; i < 6; i++)
            {
                if (i != ActiveItemIndex && GetItemAtSlotIndex(i) != null)
                {
                    int distanceToActiveIndex = GetDistanceBetweenItemSlots(ActiveItemIndex, i, ascendingOrder);

                    if (distanceToActiveIndex < closestSlotDistance)
                    {
                        closestSlotDistance = distanceToActiveIndex;
                        newItemIndex = i;
                    }
                }
            }
            // Переключение на новый индекс предмета
            SwitchToItemIndex(newItemIndex);
        }

        public void SwitchToItemIndex(int newItemIndex, bool force = false)
        {
            if (force || (newItemIndex != ActiveItemIndex && newItemIndex >= 0))
            {
                // "Анимация" переключения предмета
                _itemSwitchNewItemIndex = newItemIndex;
                _timeStartedItemSwitch = Time.time;
                if (GetActiveItem() == null)
                {
                    _itemSwitchState = ItemSwitchState.PutUpNew;
                    ActiveItemIndex = _itemSwitchNewItemIndex;

                    Item newItem = GetItemAtSlotIndex(_itemSwitchNewItemIndex);
                    _eventBus.Invoke(new OnSwitchItemSignal(newItem, ActiveItemIndex));
                }
                else
                {
                    _itemSwitchState = ItemSwitchState.PutDownPrevious;
                }
            }
        }
        public Item HasItem(Item itemPrefab)
        {
            // Проверяет есль ли предмет из Prefab
            for (var index = 0; index < _itemSlots.Length; index++)
            {
                var w = _itemSlots[index];
                if (w != null && w.SourcePrefab == itemPrefab.gameObject)
                {
                    return w;
                }
            }

            return null;
        }
        public bool AddItem(Item itemPrefab)
        {
            //Найти первое свободный слот в инвенторе

            for (int i = 0; i < _itemSlots.Length; i++)
            {
                //Добавить предмет в слот, если оно свободно
                if (_itemSlots[i] == null)
                {

                    _itemSlots[i] = InstantiateItem(itemPrefab);
                    return true;
                }
            }

            if (GetActiveItem() == null)
            {
                SwitchItem(true);
            }
            return false;
        }

        public void AddItemByIndex(Item item, int index)
        {
            ///Если слот под индексом не пустой, то удалить прошлый предмет
            if (_itemSlots[index] != null)
            {
                Destroy(_itemSlots[index].gameObject);
            }

            ///Создать новый предмет в слоте под индексом
            _itemSlots[index] = InstantiateItem(item);

            ///Если нет активного предмета - переключиться на новый
            if (GetActiveItem() == null)
            {
                SwitchItem(true);
            }
        }

        public void RemoveItemByIndex(Item item, int index)
        {
            ///удалить прошлый предмет и обнулить слот под индексом
            Destroy(_itemSlots[index].gameObject);
            _itemSlots[index] = null;

            ///Если нет активного предмета - переключиться на новый
            if (index == ActiveItemIndex)
            {
                SwitchItem(true);
            }
        }

        private void AddItemByIndex(AddItemByIndexSignal signal)
        {
            AddItemByIndex(signal.Item, signal.Index);
        }
        private void RemoveItemByIndex(RemoveItemByIndexSignal signal)
        {
            RemoveItemByIndex(signal.Item, signal.Index);
        }

        private Item InstantiateItem(Item item)
        {
            Item itemInstance = Instantiate(item, _itemParentSocket);
            itemInstance.transform.localPosition = Vector3.zero;
            itemInstance.transform.localRotation = Quaternion.identity;

            itemInstance.Owner = gameObject;
            itemInstance.SourcePrefab = item.gameObject;
            itemInstance.ShowItem(false);

            int layerIndex =
                Mathf.RoundToInt(Mathf.Log(_itemLayer.value, 2));

            foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = layerIndex;
            }
            return itemInstance;

        }


        private void TryAddItem(TryAddItemSignal signal)
        {
            Item newItem = signal.Item;

            AddItem(newItem);
        }

        public bool RemoveItem(Item itemInstance)
        {
            // Найти в слотах предмет
            for (int i = 0; i < _itemSlots.Length; i++)
            {
                // если найдено - удалить
                if (_itemSlots[i] == itemInstance)
                {
                    _itemSlots[i] = null;

                    _eventBus.Invoke(new RemoveItemSignal(itemInstance, i));

                    Destroy(itemInstance.gameObject);

                    // Переключиться на следующее оружие
                    if (i == ActiveItemIndex)
                    {
                        SwitchItem(true);
                    }

                    return true;
                }
            }

            return false;
        }
        public Item GetActiveItem()
        {
            return GetItemAtSlotIndex(ActiveItemIndex);
        }

        public Item GetItemAtSlotIndex(int index)
        {
            // Найдите активный предмет в слотах для предмета на основе нашего индекса активного предмета
            if (index >= 0 &&
                index < _itemSlots.Length)
            {
                return _itemSlots[index];
            }

            // Если не нашелся активный предмет в слотах => null
            return null;
        }
        // Обновляет анимацию смены предмета
        private void UpdateItemSwitching()
        {
            float switchingTimeFactor = 0f;
            if (_itemSwitchDelay == 0f)
            {
                switchingTimeFactor = 1f;
            }
            else
            {
                switchingTimeFactor = Mathf.Clamp01((Time.time - _timeStartedItemSwitch) / _itemSwitchDelay);
            }

            if (switchingTimeFactor >= 1f)
            {
                if (_itemSwitchState == ItemSwitchState.PutDownPrevious)
                {
                    // Деактивация старого предмета
                    Item oldItem = GetItemAtSlotIndex(ActiveItemIndex);
                    if (oldItem != null)
                    {
                        oldItem.ShowItem(false);
                    }

                    ActiveItemIndex = _itemSwitchNewItemIndex;
                    switchingTimeFactor = 0f;

                    //Активация нового предмета
                    Item newItem = GetItemAtSlotIndex(ActiveItemIndex);

                    _eventBus.Invoke(new OnSwitchItemSignal(newItem, ActiveItemIndex));

                    if (newItem)
                    {
                        _timeStartedItemSwitch = Time.time;
                        _itemSwitchState = ItemSwitchState.PutUpNew;
                    }
                    else
                    {
                        // Если новый предмет пустой, не устанавливать его обратно
                        _itemSwitchState = ItemSwitchState.Down;
                    }
                }
                else if (_itemSwitchState == ItemSwitchState.PutUpNew)
                {
                    _itemSwitchState = ItemSwitchState.Up;
                }

                // if (_itemSwitchState == itemSwitchState.PutDownPrevious)
                // {
                //     _itemMainLocalPosition = Vector3.Lerp(_defaultitemPosition.localPosition,
                //         DownitemPosition.localPosition, switchingTimeFactor);
                // }
                // else if (_itemSwitchState == itemSwitchState.PutUpNew)
                // {
                //     m_itemMainLocalPosition = Vector3.Lerp(DownitemPosition.localPosition,
                //         DefaultitemPosition.localPosition, switchingTimeFactor);
                // }
            }
        }
        
        private int GetDistanceBetweenItemSlots(int fromSlotIndex, int toSlotIndex, bool ascendingOrder)
        {
            int distanceBetweenSlots = 0;

            if (ascendingOrder)
            {
                distanceBetweenSlots = toSlotIndex - fromSlotIndex;
            }
            else
            {
                distanceBetweenSlots = -1 * (toSlotIndex - fromSlotIndex);
            }

            if (distanceBetweenSlots < 0)
            {
                distanceBetweenSlots = _itemSlots.Length + distanceBetweenSlots;
            }

            return distanceBetweenSlots;
        }

        private void OnSwitchedItem(OnSwitchItemSignal signal)
        {
            var _newItem = signal.Item;

            if (_newItem != null)
            {
                _newItem.ShowItem(true);
            }
        }


        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnSwitchItemSignal>(OnSwitchedItem);
            _eventBus.Unsubscribe<TryAddItemSignal>(TryAddItem);
            _eventBus.Unsubscribe<RemoveItemByIndexSignal>(RemoveItemByIndex);
            _eventBus.Unsubscribe<AddItemByIndexSignal>(AddItemByIndex);
        }
    }
}

