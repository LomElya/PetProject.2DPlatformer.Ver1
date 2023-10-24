using CustomEventBus.Signals;
using Platformer.Mechanics;
using UnityEngine;

namespace Interactables
{
    public class HealthItem : Interactable
    {
        [Header("Параметры")]
        [SerializeField] private float _addHealthValue = 5;
        private Health _health;

        protected override void Interact(PlayerController playerController )
        {
           
            _health = playerController.GetComponent<Health>();
            if (!_health)
            {
                _health = playerController.GetComponentInParent<Health>();
            }
            _health.Heal(_addHealthValue);
            
            PlayPickupSound();
            EffectPickup(); 
            //_eventBus.Invoke(new AddHealthSignal(_addHealthValue));
            Hide();
        }
        protected override void outSideInteract()
        {

        }
    }
}

