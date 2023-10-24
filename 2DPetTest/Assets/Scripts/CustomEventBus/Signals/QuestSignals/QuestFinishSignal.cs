namespace CustomEventBus.Signals
{
    /// <summary>
    /// Сигнал о том, что квест завершён
    /// </summary>
    public class QuestFinishSignal
    {
        public readonly QuestData QuestData;

        public QuestFinishSignal(QuestData questData)
        {
            QuestData = questData;
        }
    }
}
