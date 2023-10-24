using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Enemies;


/// <summary>
/// Система отвечающая за атаку
/// </summary>
public class AttackController : IService, IDisposable
{

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private LayerMask _enemy;
    [SerializeField] private float _damageValue;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _timeBtwAttack;
    [SerializeField] private float _timer;
    [SerializeField] private bool _canAttack = true;

    private EventBus _eventBus;
    private void Update()
    {
        AttackTimer();
    }

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<PlayerAttackSignal>(PlayerAttack);
    }
    private void PlayerAttack(PlayerAttackSignal signal)
    {
        var _animator = signal.Animator;
        _damageValue = signal.DamageValue;
        if (_canAttack)
        {
            _animator.SetTrigger("isAttack");
           /*  Collider2D[] _enemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemy);
            if (_enemies.Length != 0)
            {
                for (int i = 0; i < _enemies.Length; i++)
                {
                    _enemies[i].GetComponent<Enemy>();
                    _eventBus.Invoke(new EnemyDamageSignal(_damageValue, _enemies[i])); 
                }
            } */
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
    private void AttackTimer()
    {
        if (_timer <= 0)
        {
            _canAttack = true;
            _timer = _timeBtwAttack;
        }
        else
        {
            _canAttack = false;
            _timer -= Time.deltaTime;
        }
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<PlayerAttackSignal>(PlayerAttack);
    }

}
