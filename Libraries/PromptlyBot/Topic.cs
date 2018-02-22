using Microsoft.Bot.Builder;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PromptlyBot
{
    [DataContract()]
    public abstract class Topic
    {
        public abstract Task OnReceiveActivity(IBotContext context);

        internal Action<IBotContext> _onSuccess;
        // TODO: Remove private set, unless needed for DataContract.
        [DataMember]
        public Action<IBotContext> OnSuccess { get => _onSuccess; private set => _onSuccess = value; }


        internal Action<IBotContext, string> _onFailure;
        [DataMember]
        public Action<IBotContext, string> OnFailure { get => _onFailure; private set => _onFailure = value; }
    }

    public static class TopicExtension
    {
        public static T SetOnSuccess<T>(this T topic, Action<IBotContext> onSuccess) where T : Topic
        {
            topic._onSuccess = onSuccess;
            return topic;
        }

        public static T SetOnFailure<T>(this T topic, Action<IBotContext, string> onFailure) where T : Topic
        {
            topic._onFailure = onFailure;
            return topic;
        }
    }

    [DataContract]
    public abstract class Topic<TValue>
    {
        public abstract Task OnReceiveActivity(IBotContext context);

        internal Action<IBotContext, TValue> _onSuccess;
        // TODO: Remove private set, unless needed for DataContract.
        [DataMember]
        public Action<IBotContext, TValue> OnSuccess { get => _onSuccess; private set => _onSuccess = value; }

        internal Action<IBotContext, string> _onFailure;
        [DataMember]
        public Action<IBotContext, string> OnFailure { get => _onFailure; private set => _onFailure = value; }
    }

    public static class TopicTValueExtension
    {
        public static T SetOnSuccess<T, V>(this T topic, Action<IBotContext, V> onSuccess) where T: Topic<V>
        {
            topic._onSuccess = onSuccess;
            return topic;
        }

        public static T SetOnFailure<T, V>(this T topic, Action<IBotContext, string> onFailure) where T : Topic<V>
        {
            topic._onFailure = onFailure;
            return topic;
        }
    }
}
