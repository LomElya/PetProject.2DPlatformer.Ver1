using UI.Dialogs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragAndDropItemEquipped : DragAndDropItem
{

    protected override void SetObjectsInTimeOnDrag(Color color, bool isVisible)
    {
        _iconImage.color = color;

        _iconImage.raycastTarget = isVisible;
    }
    protected override void ChangeItemInSlot(PointerEventData eventData)
    {
        Transform objectHandler = eventData.pointerCurrentRaycast.gameObject.transform;

        InventorySlotEquipped slotEquipped = objectHandler.GetComponent<InventorySlotEquipped>();
        InventorySlotEquipped slotEquippedParent = objectHandler.parent.parent.GetComponent<InventorySlotEquipped>();

        if (objectHandler == null || objectHandler.parent.parent == null)
            return;

        if (isEquppedSlot(slotEquipped, slotEquippedParent))
        {
            if (slotEquipped != null)
            {
                if (_oldSlot._item.ItemData.TypeItem != slotEquipped.ItemType)
                    return;
                ExchangeSlotData(slotEquipped);
            }
            else if (slotEquippedParent != null)
            {
                if (_oldSlot._item.ItemData.TypeItem != slotEquippedParent.ItemType)
                    return;
                ExchangeSlotData(slotEquippedParent);
            }
        }
        else
        {
            InventorySlot slot = objectHandler.GetComponent<InventorySlot>();
            InventorySlot slotParent = objectHandler.parent.parent.GetComponent<InventorySlot>();


            if (objectHandler == null || objectHandler.parent.parent == null)
            {
                return;
            }
            else if (slot != null && slot._isEmptySlot)
            {
                ExchangeSlotData(slot);
            }
            else if (slotParent != null && slotParent._isEmptySlot)
            {
                ExchangeSlotData(slotParent);
            }

        }
    }


}