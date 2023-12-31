using System.Collections.Generic;
using System.Linq;
using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UI.Dialogs;

public class ConfigDataLoader : IService
{
    private List<ILoader> _loaders;
    private EventBus _eventBus;

    private int _loadedSystems = 0;

    public void Init(List<ILoader> loaders)
    {
        _loaders = loaders;
        
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<DataLoadedSignal>(OnConfigLoaded);
        
        // Если загрузка требует некоторого времени
        // придётся показать экран Please Wait
        if (_loaders.Any(x => !x.IsLoadingInstant()))
        {
            DialogsManager.ShowDialog<WindowLoading>();
        }
        
        LoadAll();
    }

    private void OnConfigLoaded(DataLoadedSignal signal)
    {
        _loadedSystems++;
        
        _eventBus.Invoke(new LoadProgressChangedSignal(((float)_loadedSystems/_loaders.Count)));
        if (_loadedSystems == _loaders.Count)
        {
            _eventBus.Invoke(new AllDataLoadedSignal());
        }
    }

    private void LoadAll()
    {
        foreach (var loader in _loaders)
        {
            loader.Load();
        }
    }
}
