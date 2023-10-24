using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Enemies;
using Platformer.Mechanics;


public class EnemyController : MonoBehaviour, IService, IDisposable
{
    [SerializeField] private float _speedKoef;

    //public Slime_Blue _slimeBlue;
    private Enemy _enemy;
    public Enemy Enemy => _enemy;

    public float SpeedKoef => _speedKoef;
    private bool controlEnabled = true;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<GamePauseSignal>(x => { controlEnabled = false; });
        _eventBus.Subscribe<GameUnPauseSignal>(x => { controlEnabled = true; });

        _eventBus.Subscribe<GameStartedSignal>(GameStarted);
        _eventBus.Subscribe<DamageSignal>(OnDamage);
        _eventBus.Subscribe<EnemyDeadSignal>(TryDeadEnemy);
        _eventBus.Subscribe<DieSignal>(OnDie);
        _eventBus.Subscribe<EnemyDetectSignal>(OnDetectTarget);
        _eventBus.Subscribe<EnemyLostTargetSignal>(OnLostTarget);
    }

    private void GameStarted(GameStartedSignal signal)
    {

    }
    private void OnDamage(DamageSignal signal)
    {
        var damageSource = signal.DamageSource;
        var hurt = signal.Hurt;

        if (hurt && (hurt.GetComponent<Enemy>() || !damageSource.GetComponentInParent<Enemy>()))
        {
            Enemy enemy = hurt.GetComponent<Enemy>();
            if (enemy == null)
                enemy = hurt.GetComponentInParent<Enemy>();

            enemy.onHit(damageSource, signal.Damage);
        }
    }

    private void OnDie(DieSignal signal)
    {
        var chatacter = signal.Chatacter;
        if (chatacter && !chatacter.GetComponent<PlayerController>())
        {
            Enemy enemy = chatacter.GetComponent<Enemy>();
            if (enemy == null)
                enemy = chatacter.GetComponentInChildren<Enemy>();
            ///создавать эффект после смерти
            enemy.Die();

            ///Удалить врага из списка в EmenyManager
            enemy._enemyManager.UnregisterEnemy(enemy);

            /// Создать лут из побежденного врага
            foreach (var LootItem in enemy.LootItem)
            {
                if (enemy.TryDropItem(LootItem))
                {
                    Instantiate(LootItem._lootPregabItem, enemy.transform.position, Quaternion.identity);
                    enemy.transform.position = new Vector2(enemy.transform.position.x + 0.1f, enemy.transform.position.y);
                }
            }
            /// Уничтожить врага
            //Destroy(enemy);
            enemy.Hide();

            /// Или уничтожить через время
            // Destroy(gameObject, DeathDuration);
            //enemy.Hide(DeathDuration);
        }
    }

    private void OnDetectTarget(EnemyDetectSignal signal)
    {
        Enemy enemy = signal.Enemy;
        enemy.EnemyMover.OnDetectTarget();
    }
    private void OnLostTarget(EnemyLostTargetSignal signal)
    {
        Enemy enemy = signal.Enemy;
        enemy.EnemyMover.OnLostTarget();
    }

    private void TryDeadEnemy(EnemyDeadSignal signal)
    {
        var enemy = signal.Enemy;
        enemy.Hide();
    }

    private bool TryDropItem()
    {
        return true;
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<GamePauseSignal>(x => { controlEnabled = false; });
        _eventBus.Unsubscribe<GameUnPauseSignal>(x => { controlEnabled = true; });
        _eventBus.Unsubscribe<DamageSignal>(OnDamage);
        _eventBus.Unsubscribe<EnemyDeadSignal>(TryDeadEnemy);
        _eventBus.Unsubscribe<DieSignal>(OnDie);
        _eventBus.Unsubscribe<GameStartedSignal>(GameStarted);
        _eventBus.Unsubscribe<EnemyDetectSignal>(OnDetectTarget);
        _eventBus.Unsubscribe<EnemyLostTargetSignal>(OnLostTarget);
    }
}
