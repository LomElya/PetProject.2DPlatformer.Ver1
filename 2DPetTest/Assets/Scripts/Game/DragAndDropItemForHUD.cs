using UI.Dialogs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropItemForHUD : DragAndDropItem
{
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private Text _nameText;
    [SerializeField] private Image _amountImage;

    protected override void SetObjectsInTimeOnDrag(Color color, bool isVisible)
    {
        _iconImage.color = color;

        _iconImage.raycastTarget = isVisible;
        _backgroundPanel.SetActive(isVisible);
        _nameText.gameObject.SetActive(isVisible);
        _amountImage.gameObject.SetActive(isVisible);
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
            if (slotEquipped != null && _oldSlot._item.ItemData.TypeItem == slotEquipped.ItemType)
            {
                ExchangeSlotData(slotEquipped);
            }
            else if (slotEquippedParent != null && _oldSlot._item.ItemData.TypeItem == slotEquippedParent.ItemType)
            {
                ExchangeSlotData(slotEquippedParent);
            }
        }
        else
        {
            base.ChangeItemInSlot(eventData);
        }
    }
}
