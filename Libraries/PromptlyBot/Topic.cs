using Microsoft.Bot.Builder;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PromptlyBot
{
    public interface ITopic<TValue>
    {
        Task OnReceiveActivity(IBotContext context);

        Action<IBotContext, TValue> OnSuccess { get; set; }

        Action<IBotContext, string> OnFailure { get; set; }
    }

    public interface IState
    {
        object State { get; set; }
    }

    public abstract class Topic<TState, TValue> : ITopic<TValue>, IState
    {
        private TState _state;
        public object State { get => _state; set => _state = (TState)value; }

        public Topic(TState state)
        {
            _state = state;
        }

        public abstract Task OnReceiveActivity(IBotContext context);

        private Action<IBotContext, TValue> _onSuccess;
        public Action<IBotContext, TValue> OnSuccess { get => _onSuccess; set => _onSuccess = value; }


        private Action<IBotContext, string> _onFailure;
        public Action<IBotContext, string> OnFailure { get => _onFailure; set => _onFailure = value; }
    }

    /*public static class TopicExtension
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
    }*/

    //[Serializable]
    //[DataContract]
    /*public abstract class Topic<TState, TValue> : Topic<TState>
    {
        public Topic(TState state) : base(state) { }

        private Action<IBotContext, TValue> _onSuccess;
        // TODO: Remove private set, unless needed for DataContract.
        [DataMember]
        public new Action<IBotContext, TValue> OnSuccess { get => _onSuccess; set => _onSuccess = value; }
    }*/

    /*public static class TopicTValueExtension
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
    }*/
}
