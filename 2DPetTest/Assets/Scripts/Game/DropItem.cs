using UnityEngine;

namespace Items
{
    [System.Serializable]
    public class DropItem
    {
        [Header("Префаб предмета для лута")]
        public GameObject _lootPregabItem;
        [Header("Шанс выпадения")]
        [Range(0, 1)]
        public float _dropRate = 1f;

    }
}
