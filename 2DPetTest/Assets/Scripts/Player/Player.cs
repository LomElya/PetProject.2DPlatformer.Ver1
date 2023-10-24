using System.Collections;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using Platformer.Mechanics;
using Items.Weapons;

public class Player : MonoBehaviour, IService
{

    [Header("Параметры:")]
    [SerializeField] private float _maxSpeedOnGround = 0.7f;
    [SerializeField] private float _maxSpeedInAir = 1f;
    [SerializeField] private float _jumpTakeOffSpeed = 2f;

    public float MaxSpeedOnGround => _maxSpeedOnGround;
    public float MaxSpeedInAir => _maxSpeedInAir;
    public float JumpTakeOffSpeed => _jumpTakeOffSpeed;

    [Header("Разное:")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayersItemManager _playerItemManager;
    [SerializeField] private Health _playerHealth;
    [Header("Данные для сохранения:")]
    public VectorValue mainInformation;
    private EventBus _eventBus;



    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        /* _playerController = GetComponent<PlayerController>();
        _playerWeaponManager = GetComponent<PlayersWeaponManager>();
        _playerHealth = GetComponent<Health>(); */

        _playerController.Init(this);
        _playerItemManager.Init();
        _playerHealth.Init(gameObject);

        _eventBus.Subscribe<GameStartedSignal>(GameStarted);
        _eventBus.Subscribe<DamageSignal>(OnDamage);
        _eventBus.Subscribe<GameStopSignal>(GameStop);
    }

    private void GameStarted(GameStartedSignal signal)
    {
        PlayerPrefs.SetFloat(StringConstants.CURRENT_HEALTH_PLAYER, mainInformation.currentHealth);
        PlayerPrefs.SetFloat(StringConstants.MAX_HEALTH_PLAYER, mainInformation.maxHealth);

        //_health = PlayerPrefs.GetFloat(StringConstants.CURRENT_HEALTH_PLAYER);
        //_maxHealth = PlayerPrefs.GetFloat(StringConstants.MAX_HEALTH_PLAYER);

        // _eventBus.Invoke(new HealthChangedSignal(_health, _maxHealth));
    }

    private void GameStop(GameStopSignal signal)
    {

    }
    private void OnDamage(DamageSignal signal)
    {

        var damageSource = signal.DamageSource;
        var hurt = signal.Hurt;
        if (hurt && (damageSource.GetComponent<PlayerController>() ||
        hurt.GetComponentInParent<PlayerController>()))
        {
            _eventBus.Invoke(new AddDamageSignal(signal.Damage, transform.position));
            _playerController.onHit();
        }
    }



    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GameStartedSignal>(GameStarted);
        _eventBus.Unsubscribe<DamageSignal>(OnDamage);
        _eventBus.Unsubscribe<GameStopSignal>(GameStop);
    }
}
