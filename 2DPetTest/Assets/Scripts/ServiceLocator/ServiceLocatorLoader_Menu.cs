using System.Collections.Generic;
using CustomEventBus;
using UI;
using UnityEngine;

namespace Examples.PlatformerExample
{
    public class ServiceLocatorLoader_Menu : MonoBehaviour
    {
        [SerializeField] private GUIHolder _guiHolder;

        private EventBus _eventBus;
        private СoinController _coinController;

        private List<IDisposable> _disposables = new List<IDisposable>();

        public void Awake()
        {
            _eventBus = new EventBus();
            _coinController = new СoinController();

            Register();
            Init();
            AddToDisposables();
        }

        private void Register()
        {
            ServiceLocator.Initialize();
            ServiceLocator.Current.Register(_coinController);
            //ServiceLocator.Current.Register(_scoreController);
            ServiceLocator.Current.Register(_eventBus);
            ServiceLocator.Current.Register<GUIHolder>(_guiHolder);
        }

        private void Init()
        {
            _coinController.Init();
        }

        private void AddToDisposables()
        {
            _disposables.Add(_coinController);
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

