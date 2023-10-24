using Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogs
{
    public class InventotySlotForHUD : InventorySlot
    {
        [Header("Номер кнопок:")]
        [SerializeField] private GameObject _rootNumber;
        [SerializeField] private Text _numberText;
        [SerializeField] private int _indexSlot = 1;

        public override bool TryItemAdd(Item item)
        {
            if (item != null)
            {
                _buttonSlot.onClick.AddListener(OpenInfoSlot);

                _isEmptySlot = false;

                _item = item;
                if (_item.ItemData.MaximumAmountInSlot <= 1)

                    _amountImage.gameObject.SetActive(false);
                else
                {
                    _amountImage.gameObject.SetActive(true);
                    _amountText.text = _amount.ToString();
                }

                _nameItem.text = _item.ItemData.NameItem;
                _itemIcon.sprite = _item.ItemData.IconItem;

                _rootSlot.gameObject.SetActive(true);

                _rootNumber.SetActive(true);
                _numberText.text = (_itemCounterIndex + 1).ToString();
                _indexSlot = _itemCounterIndex;
                return true;
            }

            return false;
        }

        public override void RemoveItem(Item item)
        {
            base.RemoveItem(item);
            _rootNumber.SetActive(false);
        }

        public override void SelectItem()
        {
            base.SelectItem();
            _rootNumber.SetActive(false);
        }

        public override void NotSelectItem()
        {
            base.NotSelectItem();
            _rootNumber.SetActive(true);
        }

        private ColorBlock ChangeTransparency(ColorBlock color, float transparency)
        {
            Color normalColor = color.normalColor;
            normalColor.a = transparency;

            color.normalColor = normalColor;

            return color;
        }
    }

}
