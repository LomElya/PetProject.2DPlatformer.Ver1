using System.Collections.Generic;
using UnityEngine;
using Items;

namespace Examples.PlatformerExample.Scripts.Items
{
    [CreateAssetMenu(fileName = "ItemDataConfig", menuName = "ScriptableObjects/ItemtDataConfig")]
    public class ItemDataConfig : ScriptableObject
    {
        [SerializeField] private List<ItemData> _itemData;

        public List<ItemData> ItemData => _itemData;
    }
}
