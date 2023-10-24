using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CustomEventBus;
using CustomEventBus.Signals;


namespace UI.Dialogs
{
    /// <summary>
    /// Окно главного меню 
    /// </summary>
    public class WindowMainMenu : DialogCore
    {
        [SerializeField] private Button _playNewGameButton;
        [SerializeField] private Button _playContinueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        private bool _gameStart = false;
        private EventBus _eventBus;
        public VectorValue mainInformation;
        protected override void Awake()
        {
            base.Awake();

            _playNewGameButton.onClick.AddListener(OnPlayNewGameButtonClick);
            _playContinueButton.onClick.AddListener(OnPlayContinueButtonClick);
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);

            _eventBus = ServiceLocator.Current.Get<EventBus>();
            _eventBus.Subscribe<GameStartedSignal>(x => { _gameStart = true; });

            _eventBus.Subscribe<GameStopSignal>(x => { _gameStart = false; });
        }

        private void OnPlayNewGameButtonClick()
        {
            var confirm = DialogsManager.ShowDialog<ConfirmWindow>();
            confirm.Init("Вы точно хотите начать новую игру?", () =>
            {
                /* PlayerPrefs.SetFloat(StringConstants.CURRENT_HEALTH_PLAYER, mainInformation.startingHealth);
                PlayerPrefs.SetFloat(StringConstants.MAX_HEALTH_PLAYER, mainInformation.startingMaxHealth); */
                mainInformation.maxHealth = mainInformation.startingHealth;
                mainInformation.currentHealth = mainInformation.startingHealth;
                mainInformation.currentCoin = mainInformation.startingCoin;

                SceneManager.LoadScene(StringConstants.GAME_SCENE_NAME_LV1);
            });
        }
        private void OnPlayContinueButtonClick()
        {
            Debug.Log("Игра начата? " + _gameStart);
            if (_gameStart)
            {
                Hide();
                return;
            }
            var confirm = DialogsManager.ShowDialog<ConfirmWindow>();
            confirm.Init("Продолжить игру?", () =>
            {

                SceneManager.LoadScene(StringConstants.GAME_SCENE_NAME_LV1);
            });
        }
        private void OnSettingsButtonClick()
        {
            var settings = DialogsManager.ShowDialog<SettingsWindow>();
        }
        private void OnExitButtonClick()
        {

        }
    }

}

