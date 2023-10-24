using Interactables;
using UnityEngine;

namespace Items
{

    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemtData")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private ItemType _typeItem;
        [Header("Основное")]
        [SerializeField] private int _idItem;
        [SerializeField] private string _nameItem;
        [SerializeField] private string _description;
        [SerializeField] private int _priceItem;
        [Range(1, 10)]
        [SerializeField] private int _maximumAmountInSlot;
        [Header("Спрайты и префабы")]
        [SerializeField] private Sprite _iconItem;
        [SerializeField] private Sprite _spriteItem;
        [SerializeField] private Item _prefabItem;
        [SerializeField] private ItemInteractable _prefabInteractableItem;
        public ItemType TypeItem => _typeItem;
        public int IdItem => _idItem;
        public string NameItem => _nameItem;
        public string Description => _description;
        public int PriceItem => _priceItem;
        public int MaximumAmountInSlot => _maximumAmountInSlot;
        public Sprite IconItem => _iconItem;
        public Sprite SpriteItem => _spriteItem;
        public Item PrefabItem => _prefabItem;
        public ItemInteractable PrefabInteractableItem => _prefabInteractableItem;

        public ItemData(int idItem, string nameItem, string description, int maximumAmountInSlot, int priceItem, Sprite iconItem, Sprite spriteItem)
        {
            _idItem = idItem;
            _nameItem = nameItem;
            _description = description;
            _priceItem = priceItem;
            _iconItem = iconItem;
            _spriteItem = spriteItem;
            _maximumAmountInSlot = maximumAmountInSlot;
        }
    }
}
