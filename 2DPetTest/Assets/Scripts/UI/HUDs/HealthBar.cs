using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CustomEventBus;
using CustomEventBus.Signals;
using Platformer.Mechanics;

namespace UI
{
    /// <summary>
    /// Полоска здоровья игрока(кол-во жизней)
    /// </summary>
    public class HealthBar : MonoBehaviour, IService
    {

        [SerializeField] private float _maxHearts;
        [SerializeField] private float _hearts;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Text _heartsText;
        [SerializeField] private Text _maxHeartsText;

        private Health _playerHealth;

        private EventBus _eventBus;

        public void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
            _playerHealth = playerController.GetComponent<Health>();

            _maxHearts = _playerHealth.MaxHealth;
            _hearts = _playerHealth.CurrentHealth;
            _healthBar.fillAmount = _hearts / _maxHearts;

            _eventBus.Subscribe<HealthChangedSignal>(DisplayHealth);
            // _eventBus.Subscribe<AllDataLoadedSignal>(OnAllDataLoaded);
        }

        public void DisplayHealth(HealthChangedSignal signal)
        {
            _maxHearts = _playerHealth.MaxHealth;
            _hearts = _playerHealth.CurrentHealth;

            float _healthInPercentages = _hearts / _maxHearts;

            _heartsText.text = _hearts.ToString();
            _maxHeartsText.text = "/ " + _maxHearts;
            _healthBar.fillAmount = _healthInPercentages;
        }
        private void OnDestroy()
        {
            _eventBus.Unsubscribe<HealthChangedSignal>(DisplayHealth);
        }
    }
}

