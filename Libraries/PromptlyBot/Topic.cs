using Microsoft.Bot.Builder;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PromptlyBot
{
    public interface ITopic
    {
        object State { get; set; }

        Task OnReceiveActivity(IBotContext context);

        Action<IBotContext> OnSuccess { get; set; }
 
        Action<IBotContext, string> OnFailure { get; set; }
    }

    public abstract class Topic<TState> : ITopic where TState : new()
    {
        protected TState _state;
        object ITopic.State { get => _state; set => State = (TState)value; }
        public TState State { get => _state; set => _state = value; }

        public Topic()
        {
            _state = new TState();
        }

        public abstract Task OnReceiveActivity(IBotContext context);

        private Action<IBotContext> _onSuccess;
        public Action<IBotContext> OnSuccess { get => _onSuccess; set => _onSuccess = value; }
        public ITopic SetOnSuccess(Action<IBotContext> onSuccess)
        {
            _onSuccess = onSuccess;
            return this;
        }

        private Action<IBotContext, string> _onFailure;
        public Action<IBotContext, string> OnFailure { get => _onFailure; set => _onFailure = value; }
        public Topic<TState> SetOnFailure(Action<IBotContext, string> onFailure)
        {
            _onFailure = onFailure;
            return this;
        }
    }

    public abstract class Topic<TState, TValue> : Topic<TState> where TState : new()
    {
        private Action<IBotContext, TValue> _onSuccessValue;
        new public Action<IBotContext, TValue> OnSuccess { get => _onSuccessValue; set => _onSuccessValue = value; }
        public Topic<TState, TValue> SetOnSuccess(Action<IBotContext, TValue> onSuccess)
        {
            _onSuccessValue = onSuccess;
            return this;
        }
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
