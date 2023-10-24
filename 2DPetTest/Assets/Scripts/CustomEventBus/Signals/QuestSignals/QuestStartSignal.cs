namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о начале квеста
    /// </summary>
    public class QuestStartSignal 
    {
        public readonly QuestData QuestData;

        public QuestStartSignal(QuestData questData)
        {
            QuestData = questData;
        } 
    }
}

