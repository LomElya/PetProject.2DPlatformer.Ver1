using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Enemies;

public class StatsBarEnemyController : MonoBehaviour, IService
{
    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<HealthEnemyChangedSignal>(DisplayHealth);
    }
    public void DisplayHealth(HealthEnemyChangedSignal signal)
    {
        float _hearts = signal.Health;
        float _maxHearts = signal.MaxHealth;

        var _enemy = signal.Enemy;
        var statsBar = _enemy._statsBarEnemy;

        float _healthInPercentages = _hearts / _maxHearts;
        statsBar._healthBar.fillAmount = _healthInPercentages;
    }
    private void OnDestroy()
    {
        _eventBus.Unsubscribe<HealthEnemyChangedSignal>(DisplayHealth);
    }
}
