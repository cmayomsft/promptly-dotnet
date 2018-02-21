using System.Runtime.Serialization;

namespace PromptlyBot
{
    [DataContract()]
    public abstract class ConversationTopic : Topic
    {

        internal Topic _activeTopic;

        [DataMember]
        public Topic ActiveTopic { get => _activeTopic; private set => _activeTopic = value; }

        public bool HasActiveTopic => (ActiveTopic != null);


        public void ClearActiveTopic() => _activeTopic = null;
    }

    public static class ConversationTopicExtension
    {
        public static T SetActiveTopic<T>(this ConversationTopic conversationTopic, T topic) where T : Topic
        {
            conversationTopic._activeTopic = topic;
            return topic;
        }
    }

    [DataContract()]
    public abstract class ConversationTopic<TValue> : Topic<TValue>
    {
        internal Topic _activeTopic;

        [DataMember]
        public Topic ActiveTopic { get => _activeTopic; private set => _activeTopic = value; }

        public bool HasActiveTopic => (_activeTopic != null);

        public void ClearActiveTopic() => _activeTopic = null;
    }

    public static class ConversationTopicTValueExtension
    {
        public static T SetActiveTopic<T, V>(this ConversationTopic<V> conversationTopic, T topic) where T : Topic
        {
            conversationTopic._activeTopic = topic;
            return topic;
        }
    }
}
