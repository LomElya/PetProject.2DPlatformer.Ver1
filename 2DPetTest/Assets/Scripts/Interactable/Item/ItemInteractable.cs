using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Interactables
{
    public abstract class ItemInteractable : Interactable
    {
        [Header("Предмет:")]
        [SerializeField] protected Item _itemPrefab;
        public Item ItemPrefab => _itemPrefab;


    }
}
