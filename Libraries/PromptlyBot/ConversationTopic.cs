namespace PromptlyBot
{
    public abstract class ConversationTopic<TValue> : Topic<TValue>
    {
        public Topic<TValue> ActiveTopic { get; set; }

        public bool HasActiveTopic => (ActiveTopic != null);

        public void ClearActiveTopic() => ActiveTopic = null;
    }
}
