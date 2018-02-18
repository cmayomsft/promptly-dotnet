namespace Promptly
{
    public abstract class ConversationTopic : Topic
    {
        public Topic ActiveTopic { get; set; }

        public bool HasActiveTopic => (ActiveTopic != null);

        public void ClearActiveTopic() => ActiveTopic = null;
    }


    public abstract class ConversationTopic<TValue> : Topic<TValue>
    {
        public Topic ActiveTopic { get; set; }

        public bool HasActiveTopic => (ActiveTopic != null);

        public void ClearActiveTopic() => ActiveTopic = null;
    }
}
