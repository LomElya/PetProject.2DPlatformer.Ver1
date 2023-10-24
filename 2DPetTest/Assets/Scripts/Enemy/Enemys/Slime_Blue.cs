using UnityEngine;
using CustomEventBus.Signals;
using Interface;

namespace Enemies
{
    public class Slime_Blue : Enemy, IMove, IInteract
    {
        private Vector2 move;
        protected override string GetDescription()
        {
            return "Это голубой слайм. Его Урон: " + EnemyWeapon.Damage + ", Максимальное здоровье: " + EnemyHealth.MaxHealth + ", Броня: " + Armor;
        }
        public void Interact()
        {
            Debug.Log(GetDescription());
            //_eventBus.Invoke(new PlayerDamagedSignal(Damage));
            _eventBus.Invoke(new OnAttackSignal(EnemyWeapon));
        }
        public void Move()
        {
            
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag.Equals("Player"))
            {
                Interact();
                //_eventBus.Invoke(new EnemyDamageSignal(5, this));
            }
        }

    }
}

