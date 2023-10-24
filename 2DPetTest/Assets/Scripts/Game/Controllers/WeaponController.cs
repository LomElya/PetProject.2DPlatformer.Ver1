using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Platformer.Mechanics;
using Enemies;

public class WeaponController : IService, IDisposable
{
    public GameObject Owner { get; set; }
    public bool IsWeaponActive { get; private set; }

    const string k_AnimAttackParameter = "isAttack";

    private List<Collider2D> _ignoredColliders;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<OnAttackSignal>(OnAttack);
    }

    private void OnAttack(OnAttackSignal signal)
    {
        var weaponAttack = signal.Weapon;
        //var owner = weaponAttack.Owner;
        Owner = weaponAttack.gameObject;
        float damage = weaponAttack.Damage;

        var attackPoint = weaponAttack.AttackPoint;
        var attackRange = weaponAttack.AttackRange;
        var enemyLayers = weaponAttack.EnemyLayers;

        _ignoredColliders = new List<Collider2D>();
        Collider2D[] ownerColliders = Owner.GetComponentsInChildren<Collider2D>();
        _ignoredColliders.AddRange(ownerColliders);


        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (IsHitValid(enemy))
            {
                OnHit(enemy, damage);
            }
        }
    }
    private bool IsHitValid(Collider2D hit)
    {
        // Игнорировать попадание, если нет компонента Damageable 
        if (hit.isTrigger && hit.GetComponent<Damageable>() == null)
        {
            return false;
        }

        // Игнорировать собственный коллайдер
        /*  if (_ignoredColliders != null && _ignoredColliders.Contains(hit))
         {
             return false;
         } */

        return true;
    }
    private void OnHit(Collider2D collider, float damage)
    {
        Damageable damageable = collider.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.InflictDamage(damage, false, Owner);
            _eventBus.Invoke(new DamageSignal(damage, Owner.gameObject, damageable.gameObject));
        }

    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<OnAttackSignal>(OnAttack);
    }
}
