using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Dialogs
{
    /// <summary>
    /// Кнопка взаимодействия, которое появляется, когда игрок подходит к объекту
    /// </summary>
    public class PlayerEnterZoneEvent : DialogCore
    {
        [SerializeField] private Button _eventButton;
        [SerializeField] private Text _eventText;

        [SerializeField] private TextAsset _TXTAsset;

        private EventBus _eventBus;

        void Start()
        {
            _eventButton.onClick.AddListener(StartDialog);
            _eventBus = ServiceLocator.Current.Get<EventBus>();
        }

        public void Init(string eventText, TextAsset TXTAsset)
        {
            _eventText.text = eventText;
            _TXTAsset = TXTAsset;
            
        }

        private void StartDialog()
        {   
            _eventBus.Invoke(new PlayerStartDialogSignal(_TXTAsset));
            Hide();
        }
    }
    
}

