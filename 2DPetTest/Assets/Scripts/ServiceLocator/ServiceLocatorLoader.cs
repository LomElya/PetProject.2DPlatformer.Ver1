using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using UI;
using Enemies;
using UI.Dialogs;


namespace Examples.PlatformerExample
{
    public class ServiceLocatorLoader : MonoBehaviour
    {
        //[SerializeField] private GameCharacter _characterTask;
        [SerializeField] private StatsBarEnemyController _statsBarEnemyController;
        [SerializeField] private Player _player;
        [SerializeField] private GUIHolder _guiHolder;
        [SerializeField] private DamageUI _damageUI;
        [SerializeField] private DamageUIMover _damageUIMover;
        [SerializeField] private QuestController _questController;

        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CoinHUD _coinHUD;
        [SerializeField] private ScriptableObjectQuestLoader _scriptableObjectQuestLoader;

        private ConfigDataLoader _configDataLoader;
        private EventBus _eventBus;
        private GameController _gameController;
        private EnemyController _enemyController;
        private AttackController _attackController;
        private WeaponController _weaponController;
        private СoinController _coinController;
        private EnemyManager _enemyManager;

        private IQuestLoader _questLoader;
        private List<IDisposable> _disposables = new List<IDisposable>();
        private void Awake()
        {
            _eventBus = new EventBus();
            _gameController = new GameController();
            _enemyController = new EnemyController();
            _coinController = new СoinController();
            _weaponController = new WeaponController();
            _attackController = new AttackController();
            _enemyManager = new EnemyManager();
            _questLoader = _scriptableObjectQuestLoader;

            RegisterServices();
            Init();
            AddDisposables();
        }

        public void RegisterServices()
        {
            ServiceLocator.Initialize();

            ServiceLocator.Current.Register(_eventBus);

            // ServiceLocator.Current.Register<GameCharacter>(_character);
            ServiceLocator.Current.Register<StatsBarEnemyController>(_statsBarEnemyController);
            ServiceLocator.Current.Register<Player>(_player);
            ServiceLocator.Current.Register(_enemyManager);
            ServiceLocator.Current.Register<HealthBar>(_healthBar);
            ServiceLocator.Current.Register<GUIHolder>(_guiHolder);
            ServiceLocator.Current.Register<DamageUI>(_damageUI);
            ServiceLocator.Current.Register<DamageUIMover>(_damageUIMover);
            ServiceLocator.Current.Register(_gameController);
            ServiceLocator.Current.Register(_enemyController);
            ServiceLocator.Current.Register(_coinController);
            ServiceLocator.Current.Register(_weaponController);
            ServiceLocator.Current.Register(_attackController);
            ServiceLocator.Current.Register<IQuestLoader>(_questLoader);

        }

        public void Init()
        {
            _gameController.Init();
            _guiHolder.Init();
            //_character.Init();

            _enemyController.Init();
            _enemyManager.Init();
            _statsBarEnemyController.Init();

            _player.Init();
            //_healthBar.Init();

            _damageUI.Init();
            _damageUIMover.Init();

            //_coinHUD.Init();

            _coinController.Init();
            _questController.Init();
            _weaponController.Init();
            _attackController.Init();

            var loaders = new List<ILoader>();
            loaders.Add(_questLoader);
            _configDataLoader = new ConfigDataLoader();
            _configDataLoader.Init(loaders);
        }

        private void AddDisposables()
        {
            _disposables.Add(_gameController);
            _disposables.Add(_gameController);
            _disposables.Add(_coinController);
            _disposables.Add(_weaponController);
            _disposables.Add(_attackController);
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}

