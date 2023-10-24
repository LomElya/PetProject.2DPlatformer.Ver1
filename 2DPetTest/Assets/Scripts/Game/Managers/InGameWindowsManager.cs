using CustomEventBus;
using CustomEventBus.Signals;
using UI.Dialogs;
using UnityEngine;

public class InGameWindowsManager : MonoBehaviour
{
    [SerializeField] private GameObject Root;

    [Header("Окна:")]
    [SerializeField] PauseWindow _pauseWindow;

    private EventBus _eventBus;
    public void Start()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        Root.gameObject.SetActive(false);

        
        _pauseWindow.Init(_eventBus);

        _eventBus.Subscribe<GamePauseSignal>(x => { Root.gameObject.SetActive(true);});

        _eventBus.Subscribe<GameUnPauseSignal>(x => { Root.gameObject.SetActive(false);});
    }

    private void OnDestroy()
    {
        //_eventBus.Unsubscribe<GameStopSignal>(OpenBackground);
    }
}
