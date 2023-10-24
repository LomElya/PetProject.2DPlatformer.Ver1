using System.Linq;
using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// Отвечает за логику квестов:
/// Переключает текущий уровень на другой
/// Уведомляет остальные системы что уровень изменился
/// Уведомляет что уровень пройден
/// </summary>
public class QuestController : MonoBehaviour, IService
{
    
    private IQuestLoader _questLoader;
    private int _currentQuestId;

    private QuestData _currentQuestData;

    private EventBus _eventBus;

    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<NextQuestSignal>(NextQuest);
        //_eventBus.Subscribe<RestartLevelSignal>(RestartLevel);

        _questLoader = ServiceLocator.Current.Get<IQuestLoader>();
        _currentQuestId = PlayerPrefs.GetInt(StringConstants.CURRENT_QUEST, 0);

        OnInit();
    }
    private async void OnInit()
    {
        await UniTask.WaitUntil(_questLoader.IsLoaded);
        _currentQuestData = _questLoader.GetQuest().FirstOrDefault(x => x.IDQuest == _currentQuestId);
        if (_currentQuestData == null)
        {
            Debug.LogErrorFormat("Can't find level with id {0}", _currentQuestId);
            return;
        }
        _eventBus.Invoke(new SetQuestSignal(_currentQuestData));
    }

    private void NextQuest(NextQuestSignal signal)
    {
        //_currentQuestId++;
        SelectQuest(_currentQuestId);
    }
    private void SelectQuest(int quest)
    {
        _currentQuestId = quest;
        _currentQuestData = _questLoader.GetQuest().FirstOrDefault(x => x.IDQuest == _currentQuestId);
        _eventBus.Invoke(new SetQuestSignal(_currentQuestData));
        PlayerPrefs.SetInt(StringConstants.CURRENT_QUEST, (_currentQuestId + 1));
    }
    private void OnDestroy()
    {
        _eventBus.Unsubscribe<NextQuestSignal>(NextQuest);
        //_eventBus.Unsubscribe<LevelTimePassedSignal>(LevelPassed);
    }
}
