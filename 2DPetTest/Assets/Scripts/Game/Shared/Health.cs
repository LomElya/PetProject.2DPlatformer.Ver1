using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class Health : MonoBehaviour
{
    public float MaxHealth = 100f;

    public float CurrentHealth;
    private bool _isDead;
    private bool _isDamage;
    public bool Invincible { get; set; }
    public GameObject Owner { get; set; }
    protected EventBus _eventBus;

    public virtual void Init(GameObject owner)
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        Owner = owner;
        CurrentHealth = MaxHealth;

        _eventBus.Subscribe<OnUseHealthSignal>(UseHeal);
    }
    private void UseHeal(OnUseHealthSignal signal)
    {
        float healValue = signal.Value;
        Heal(healValue);
    }
    public virtual void Heal(float healAmount)
    {
        float healthBefore = CurrentHealth;
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        // вызвать сигнал о измнении здоровья 
        float trueHealAmount = CurrentHealth - healthBefore;
        if (trueHealAmount > 0f)
        {
            _isDamage = false;
            //_eventBus.Invoke(new HealthChangedSignal(trueDamageAmount, damageSource, _isDamage));
            _eventBus.Invoke(new HealthChangedSignal(CurrentHealth, MaxHealth, Owner));
        }
    }
    public virtual void TakeDamage(float damage, GameObject damageSource)
    {
        if (Invincible)
            return;

        float healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        // вызвать сигнал о измнении здоровья 
        float trueDamageAmount = healthBefore - CurrentHealth;
        if (trueDamageAmount > 0f)
        {
            _isDamage = true;
            //_eventBus.Invoke(new HealthChangedSignal(trueDamageAmount, damageSource, _isDamage));
            _eventBus.Invoke(new HealthChangedSignal(CurrentHealth, MaxHealth, Owner));
        }

        HandleDeath();
    }

    public virtual void Kill()
    {
        CurrentHealth = 0f;

        // вызвать сигнал о измнении здоровья
        _eventBus.Invoke(new HealthChangedSignal(CurrentHealth, MaxHealth, Owner));

        HandleDeath();
    }

    protected virtual void HandleDeath()
    {
        if (_isDead)
            return;

        // вызвать сигнал о смерти 
        if (CurrentHealth <= 0f)
        {
            _isDead = true;
            _eventBus.Invoke(new DieSignal(gameObject));
        }
    }
    private void OnDestroy()
    {
        _eventBus.Unsubscribe<OnUseHealthSignal>(UseHeal);
    }

}
