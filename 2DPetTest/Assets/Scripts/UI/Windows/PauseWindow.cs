using UnityEngine;
using UnityEngine.UI;
using CustomEventBus;
using UnityEngine.SceneManagement;
using CustomEventBus.Signals;

namespace UI.Dialogs
{
    /// <summary>
    /// Окно паузы
    /// </summary>
    public class PauseWindow : DialogCore
    {
        [SerializeField] private GameObject _pauseWindow;
        [Header("Кнопки")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        [Header("Окна")]
        private EventBus _eventBus;

        private new void Awake()
        {
            _continueButton.onClick.AddListener(onContinueButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }
        public void Init(EventBus eventBus)
        {
            _eventBus = eventBus;

            _eventBus.Subscribe<OpenPauseMenuSignal>(x => { _pauseWindow.gameObject.SetActive(true); });
            _eventBus.Subscribe<GameUnPauseSignal>(x => { _pauseWindow.gameObject.SetActive(false); });
        }


        private void onContinueButtonClicked()
        {
            _eventBus.Invoke(new GameUnPauseSignal());
            //Hide();
        }
        private void OnSettingsButtonClick()
        {
            //_pauseWindow.gameObject.SetActive(false);
            var settings = DialogsManager.ShowDialog<SettingsWindow>();
            //Hide();
        }
        private void OnExitButtonClick()
        {
            //_eventBus.Invoke(new GameExitSignal());

            var confirm = DialogsManager.ShowDialog<ConfirmWindow>();

            confirm.Init("Вы точно хотите выйти?", () =>
            {

                _eventBus.Invoke(new GameUnPauseSignal());
                SceneManager.LoadScene(StringConstants.MENU_SCENE_NAME);
                //Hide();
            });
        }
    }
}

