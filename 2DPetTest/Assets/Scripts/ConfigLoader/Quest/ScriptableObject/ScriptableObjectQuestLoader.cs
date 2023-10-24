using System.Collections.Generic;
using System.Linq;
using CustomEventBus;
using CustomEventBus.Signals;
using Examples.PlatformerExample.Scripts.Quest;
using UnityEngine;

public class ScriptableObjectQuestLoader : MonoBehaviour, IQuestLoader
{
    [SerializeField] private QuestDataConfig _questDataConfig;
    public IEnumerable<QuestData> GetQuest()
    {
        return _questDataConfig.QuestData;
    }
    public QuestData GetCurrentQuestData()
    {
        var id = PlayerPrefs.GetInt(StringConstants.CURRENT_QUEST, 0);
        return _questDataConfig.QuestData.FirstOrDefault(x => x.IDQuest == id);
    }

    public bool IsLoaded()
    {
        return true;
    }
    public void Load()
    {
        var eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Invoke(new DataLoadedSignal(this));
    }
    public bool IsLoadingInstant()
    {
        return true;
    }
    
}
