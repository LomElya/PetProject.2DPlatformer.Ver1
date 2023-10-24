using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

public  class DialogController : IService
{
    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>(); 
    }

   
}


