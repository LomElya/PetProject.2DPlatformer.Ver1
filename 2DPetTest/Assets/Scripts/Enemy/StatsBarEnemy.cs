using UnityEngine;
using UnityEngine.UI;
using CustomEventBus;
using CustomEventBus.Signals;

namespace UI
{
    /// <summary>
    /// Полоска/полоски статуса врага(кол-во жизней. "В будущем:" + маны, защиты)
    /// все действия происходят в StatsBarEnemyController
    /// </summary>
    public class StatsBarEnemy : MonoBehaviour
    {
        [SerializeField] public Image _healthBar;
        private Health _health;
        //[SerializeField] public Image _oldHealthBar;
        //[SerializeField] public Image _manaBar;
        //[SerializeField] public Image _oldManaBar;

        private EventBus _eventBus;

        public void Init(Health health)
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            _eventBus.Subscribe<HealthChangedSignal>(OnHealthChanged);

            _health = health;
            _healthBar.fillAmount = _health.CurrentHealth / _health.MaxHealth;
        }

        private void OnHealthChanged(HealthChangedSignal signal)
        {
            _healthBar.fillAmount = _health.CurrentHealth / _health.MaxHealth;
            //_healthBar.fillAmount = signal.Health / signal.MaxHealth;
        }
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<HealthChangedSignal>(OnHealthChanged);
        }

    }
}

