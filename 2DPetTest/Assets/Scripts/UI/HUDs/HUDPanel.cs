using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogs
{
    public class HUDPanel : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _bagButton;
        private bool _isPauseOpen = false;

        private EventBus _eventBus;

        public void Start()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            _pauseButton.onClick.AddListener(OpenPauseMenu);
            _bagButton.onClick.AddListener(OpenBag);

            _eventBus.Subscribe<GamePauseSignal>(x => { _isPauseOpen = true; });

            _eventBus.Subscribe<GameUnPauseSignal>(x => { _isPauseOpen = false; });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenPauseMenu();
            }
        }
        private void OpenPauseMenu()
        {
            if (isPause())
                return;

            _eventBus.Invoke(new GamePauseSignal());
            _eventBus.Invoke(new OpenPauseMenuSignal());

            //var pause = DialogsManager.ShowDialog<PauseWindow>();
        }
        private bool isPause()
        {
            return _isPauseOpen;
        }
        private void OpenBag()
        {
            if (isPause())
                return;

            _eventBus.Invoke(new OpenInventorySignal());
            _eventBus.Invoke(new GamePauseSignal());

            //var inventory = DialogsManager.ShowDialog<Inventory>();
        }
    }
}

