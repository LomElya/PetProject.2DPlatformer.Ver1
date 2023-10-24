using Items;
using UI.Dialogs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CustomEventBus;
using CustomEventBus.Signals;

public abstract class DragAndDropItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] protected InventorySlot _oldSlot;
    [SerializeField] protected Image _iconImage;
    protected RectTransform _draggableParant;
    protected Transform _mouseTransform;
    protected EventBus _eventBus;


    public void Init(RectTransform parent, EventBus eventBus)
    {
        _draggableParant = parent;
        _eventBus = eventBus;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_oldSlot._isEmptySlot)
            return;

        SetObjectsInTimeOnDrag((new Color(1, 1, 1, 0.75f)), false);
        transform.SetParent(_draggableParant);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (_oldSlot._isEmptySlot)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_oldSlot._isEmptySlot)
            return;

        //Поставить DraggableObject обратно в свой старый слот
        transform.SetParent(_oldSlot.transform);
        transform.position = _oldSlot.transform.position;

        SetObjectsInTimeOnDrag((new Color(1, 1, 1, 1f)), true);
        if (!IsOutOfBounds(eventData))
        {
            ChangeItemInSlot(eventData);
        }
    }

    protected virtual void ChangeItemInSlot(PointerEventData eventData)
    {
        Transform objectHandler = eventData.pointerCurrentRaycast.gameObject.transform;

        InventorySlot slot = objectHandler.GetComponent<InventorySlot>();
        InventorySlot slotParent = objectHandler.parent.parent.GetComponent<InventorySlot>();


        if (objectHandler == null || objectHandler.parent.parent == null)
        {
            return;
        }
        else if (slot != null)
        {
            ExchangeSlotData(slot);
        }
        else if (slotParent != null)
        {
            ExchangeSlotData(slotParent);
        }
    }
    protected bool IsOutOfBounds(PointerEventData eventData)
    {
        //Если мышка отпущена над объектом по имени UIBackground, то...
        if (eventData.pointerCurrentRaycast.gameObject.name == "UIBackground")
        {
            NullifySlotData();
            return true;
        }
        return false;
    }

    public void NullifySlotData() // made public 
    {
        // убираем значения InventorySlot
        _oldSlot.DropItem(_oldSlot._item, _oldSlot._amount);
    }
    protected void ExchangeSlotData(InventorySlot newSlot)
    {
        // Временно храним данные newSlot в отдельных переменных
        int currentAmount = newSlot._amount;
        Item item = newSlot._item;
        bool isEmpty = newSlot._isEmptySlot;
        // Заменяем значения newSlot на значения oldSlot
        if (newSlot._item == _oldSlot._item)
        {
            if (newSlot.TryAddAmount(item, 1))
            {
                _eventBus.Invoke(new RemoveItemByIndexSignal(item, _oldSlot.GetIndexSlot()));
                _oldSlot.RemoveItem(item);
                return;
            }
        }
        newSlot._item = _oldSlot._item;

        if (!_oldSlot._isEmptySlot)
        {
            newSlot._amount = _oldSlot._amount;
            _eventBus.Invoke(new AddItemByIndexSignal(newSlot._item, newSlot.GetIndexSlot()));
            newSlot.AddItem(newSlot._item);
            Debug.Log(1);
        }
        else
        {
            _eventBus.Invoke(new RemoveItemByIndexSignal(newSlot._item, newSlot.GetIndexSlot()));
            newSlot.RemoveItem(newSlot._item);
        }

        // Заменяем значения oldSlot на значения newSlot сохраненные в переменных
        if (!isEmpty)
        {
            _oldSlot._amount = currentAmount;
            _eventBus.Invoke(new AddItemByIndexSignal(item, _oldSlot.GetIndexSlot()));
            _oldSlot.AddItem(item);
            Debug.Log(2);
        }
        else
        {
            _eventBus.Invoke(new RemoveItemByIndexSignal(item, _oldSlot.GetIndexSlot()));
            _oldSlot.RemoveItem(item);
        }

    }
    protected virtual void SetObjectsInTimeOnDrag(Color color, bool isVisible)
    {

    }
    protected bool isEquppedSlot(InventorySlotEquipped slotEquipped, InventorySlotEquipped slotEquippedParent)
    {
        if (slotEquipped != null || slotEquippedParent != null)
            return true;

        return false;
    }


}
