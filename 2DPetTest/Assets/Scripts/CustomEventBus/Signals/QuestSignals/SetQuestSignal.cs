namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о изменении квеста
    /// </summary>
    public class SetQuestSignal 
    {
        public readonly QuestData QuestData;

        public SetQuestSignal(QuestData questData)
        {
            QuestData = questData;
        } 
    }
}

