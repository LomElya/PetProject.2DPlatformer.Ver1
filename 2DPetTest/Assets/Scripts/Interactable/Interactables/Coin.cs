using CustomEventBus.Signals;
using Platformer.Mechanics;
using UnityEngine;

namespace Interactables
{
    public class Coin : Interactable
    {
        [Header("Параметры")]
        [SerializeField] private int _coinValue = 2;
        protected override void Interact(PlayerController playerController)
        {
            _eventBus.Invoke(new AddCoinSignal(_coinValue));
            PlayPickupSound();
            EffectPickup(); 
            Hide();
        }
        protected override void outSideInteract()
        {

        }
    }
}

