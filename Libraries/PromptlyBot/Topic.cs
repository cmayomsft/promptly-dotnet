using Microsoft.Bot.Builder;
using System;
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
        private readonly TopicFluentInterface _set;

        public Topic() : base()
        {
            this._state = new TState();

            this._set = new TopicFluentInterface(this);
        }

        public TopicFluentInterface Set { get => _set; }

        protected TState _state;
        object ITopic.State { get => _state; set => State = (TState)value; }
        public TState State { get => _state; set => _state = value; }

        public abstract Task OnReceiveActivity(IBotContext context);

        private Action<IBotContext> _onSuccess;
        public Action<IBotContext> OnSuccess { get => _onSuccess; set => _onSuccess = value; }

        private Action<IBotContext, string> _onFailure;
        public Action<IBotContext, string> OnFailure { get => _onFailure; set => _onFailure = value; }

        public class TopicFluentInterface
        {
            private readonly Topic<TState> _topic;

            public TopicFluentInterface(Topic<TState> topic)
            {
                this._topic = topic;
            }

            public TopicFluentInterface OnSuccess(Action<IBotContext> onSuccess)
            {
                _topic._onSuccess = onSuccess;
                return this;
            }

            public TopicFluentInterface OnFailure(Action<IBotContext, string> onFailure)
            {
                _topic._onFailure = onFailure;
                return this;
            }
        }
    }

    public abstract class Topic<TState, TValue> : Topic<TState> where TState : new()
    {
        private readonly TopicValueFluentInterface _set;

        public Topic() : base()
        {
            this._set = new TopicValueFluentInterface(this);
        }

        new public TopicValueFluentInterface Set { get => _set; }

        private Action<IBotContext, TValue> _onSuccessValue;
        new public Action<IBotContext, TValue> OnSuccess { get => _onSuccessValue; set => _onSuccessValue = value; }

        public class TopicValueFluentInterface
        {
            private readonly Topic<TState, TValue> _topic;

            public TopicValueFluentInterface(Topic<TState, TValue> topic)
            {
                this._topic = topic;
            }

            public TopicValueFluentInterface OnSuccess(Action<IBotContext, TValue> onSuccess)
            {
                _topic._onSuccessValue = onSuccess;
                return this;
            }

            public TopicValueFluentInterface OnFailure(Action<IBotContext, string> onFailure)
            {
                _topic.OnFailure = onFailure;
                return this;
            }
        }
    }
}
