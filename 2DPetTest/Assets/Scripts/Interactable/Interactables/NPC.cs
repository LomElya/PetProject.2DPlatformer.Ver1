using CustomEventBus.Signals;
using Platformer.Mechanics;
using UnityEngine;

namespace Interactables
{
    public class NPC : Interactable
{
   // [SerializeField] private TextAsset _ta;
   // [SerializeField] private string _textEventButton = "Поговорить";
    [SerializeField] private int _damageValue = 10;
    protected override void Interact(PlayerController playerController)
    {
        _eventBus.Invoke(new PlayerDamagedSignal(_damageValue));
        //_eventBus.Invoke(new PlayerEnteredNPCZoneSignal(_ta, _textEventButton));
    }
    protected override void outSideInteract()
    {
        //_eventBus.Invoke(new PlayerEndDialogSignal());
    }
}
}

