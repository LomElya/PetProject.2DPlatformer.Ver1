using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Platformer.Dialogue;

namespace UI.Dialogs
{
    /// <summary>
    /// Закрывает диалог, если игрок вышел из зоны взаимодействия с NPC
    /// или закончил диалог
    /// </summary>
    public class PlayerEndDialog : DialogCore
    {
        private EventBus _eventBus;
        public void Init()
        {
            _eventBus = ServiceLocator.Current.Get<EventBus>();
            Hide();
        }
    }
}

