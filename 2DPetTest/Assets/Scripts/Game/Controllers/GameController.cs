using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UI.Dialogs;

/// <summary>
/// Принимает решение о запуске, остановке игры и начале диалога
/// Уведомляет о старте/конце игры
/// </summary>
public class GameController : MonoBehaviour, IService, IDisposable
{

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<PlayerEnteredNPCZoneSignal>(OnPlayerEnteredNPCZone);
        _eventBus.Subscribe<PlayerStartDialogSignal>(StartDialog);
        _eventBus.Subscribe<PlayerEndDialogSignal>(EndDialog);
        
        _eventBus.Invoke(new GameStartedSignal());
    }
    

    public void StopGame(GameStopSignal signal)
    {
        _eventBus.Invoke(new GameStopSignal());
    }

    public void OnPlayerEnteredNPCZone(PlayerEnteredNPCZoneSignal signal)
    {
        var TXTAsset = signal.Text;
        var textEvent = signal.TextEventButton;

        PlayerEnterZoneEvent playerEnterZoneEvent = DialogsManager.ShowDialog<PlayerEnterZoneEvent>();
        playerEnterZoneEvent.Init(textEvent, TXTAsset);
    }

    public void StartDialog(PlayerStartDialogSignal signal)
    {
        var TXTAsset = signal.Text;

        PlayerDialogNPC playerDialogNPC = DialogsManager.ShowDialog<PlayerDialogNPC>();
        playerDialogNPC.Init(TXTAsset);
    }
     public void EndDialog(PlayerEndDialogSignal signal)
    {
        PlayerEndDialog playerEndDialog = FindFirstObjectByType<PlayerEndDialog>();;
        playerEndDialog.Init();  
    } 


    public void Dispose()
    {
        _eventBus.Unsubscribe<PlayerEnteredNPCZoneSignal>(OnPlayerEnteredNPCZone);
        _eventBus.Unsubscribe<PlayerStartDialogSignal>(StartDialog);
        _eventBus.Unsubscribe<PlayerEndDialogSignal>(EndDialog);
    }
}
