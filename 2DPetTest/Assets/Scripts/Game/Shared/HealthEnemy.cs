using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using UI;

public class HealthEnemy : Health
{
    [SerializeField] private StatsBarEnemy _enemyStatBar;
    public override void Init(GameObject owner)
    {
        base.Init(owner);

        _enemyStatBar.Init(this);
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
    }
    public override void TakeDamage(float damage, GameObject damageSource)
    {
        base.TakeDamage(damage, damageSource);
    }

    public override void Kill()
    {
        base.Kill();
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();
    }

}
