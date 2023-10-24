using System.Collections;
using System.Collections.Generic;
using Items.Weapons;
using UnityEngine;
using CustomEventBus.Signals;
using Platformer.Mechanics;
using Items;

namespace Interactables
{
    public class HealthFlaskItem : ItemInteractable
    {


        protected override void Interact(PlayerController playerController)
        {
            //_eventBus.Invoke(new AddItemSignal(_itemPrefab));

            PlayersItemManager playersItemManager = playerController.GetComponent<PlayersItemManager>();
            if (playersItemManager)
            {
                //if (playersItemManager.AddItem(_itemPrefab))
                _eventBus.Invoke(new AddItemSignal(_itemPrefab));
                {
                    if (playersItemManager.GetActiveItem() == null)
                    {
                        playersItemManager.SwitchItem(true);
                    }
                }
                PlayPickupSound();
                EffectPickup();
                Hide();
            }
        }
        protected override void outSideInteract()
        {

        }
    }
}
