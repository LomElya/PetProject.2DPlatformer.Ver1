using System.Collections.Generic;
public interface IQuestLoader: IService, ILoader
{
    public IEnumerable<QuestData> GetQuest();
}
