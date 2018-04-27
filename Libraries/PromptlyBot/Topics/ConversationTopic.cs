using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;

namespace PromptlyBot
{
    public class ActiveTopicState
    {
        public string Key;
        public object State;
    }

    public class ConversationTopicState
    {
        public ActiveTopicState ActiveTopic;
    }

    public delegate ITopic CreateSubTopicDelegate(params object[] args);

    public abstract class ConversationTopic<TState> : Topic<TState> where TState : ConversationTopicState, new()
    {
        private readonly ConversationTopicFluentInterface _set;

        public ConversationTopic() : base()
        {
            this._set = new ConversationTopicFluentInterface(this);
        }

        new public ConversationTopicFluentInterface Set { get => _set; }

        private Dictionary<string, CreateSubTopicDelegate> _subTopics = new Dictionary<string, CreateSubTopicDelegate>();
        public Dictionary<string, CreateSubTopicDelegate> SubTopics { get => _subTopics; }

        private ITopic _activeTopic;
        public ITopic SetActiveTopic(string subTopicKey, params object[] args)
        {
            if (args.Length > 0)
            {
                this._activeTopic = this._subTopics[subTopicKey](args);
            }
            else
            {
                this._activeTopic = this._subTopics[subTopicKey]();
            }

            this._state.ActiveTopic = new ActiveTopicState { Key = subTopicKey, State = this._activeTopic.State };

            return this._activeTopic;
        }
        public ITopic ActiveTopic
        {
            get
            {
                if (this._state.ActiveTopic == null)
                {
                    return null;
                }

                if (this._activeTopic != null)
                {
                    return this._activeTopic;
                }

                this._activeTopic = this._subTopics[this._state.ActiveTopic.Key]();
                this._activeTopic.State = this._state.ActiveTopic.State;

                return this._activeTopic;
            }
        }

        public bool HasActiveTopic => (this._state.ActiveTopic != null);

        public void ClearActiveTopic() => this._state.ActiveTopic = null;

        public class ConversationTopicFluentInterface
        {
            private readonly ConversationTopic<TState> _ConversationTopic;

            public ConversationTopicFluentInterface(ConversationTopic<TState> conversationTopic)
            {
                this._ConversationTopic = conversationTopic;
            }

            public ConversationTopicFluentInterface OnSuccess(Action<TurnContext> onSuccess)
            {
                _ConversationTopic.OnSuccess = onSuccess;
                return this;
            }

            public ConversationTopicFluentInterface OnFailure(Action<TurnContext, string> onFailure)
            {
                _ConversationTopic.OnFailure = onFailure;
                return this;
            }
        }
    }

    public abstract class ConversationTopic<TState, TValue> : ConversationTopic<TState> where TState : ConversationTopicState, new()
    {
        private readonly ConversationTopicValueFluentInterface _set;

        public ConversationTopic() : base()
        {
            this._set = new ConversationTopicValueFluentInterface(this);
        }

        new public ConversationTopicValueFluentInterface Set { get => _set; }

        private Action<TurnContext, TValue> _onSuccessValue;
        new public Action<TurnContext, TValue> OnSuccess { get => _onSuccessValue; set => _onSuccessValue = value; }

        public class ConversationTopicValueFluentInterface
        {
            private readonly ConversationTopic<TState, TValue> _ConversationTopicValue;

            public ConversationTopicValueFluentInterface(ConversationTopic<TState, TValue> conversationTopicValue)
            {
                this._ConversationTopicValue = conversationTopicValue;
            }

            public ConversationTopicValueFluentInterface OnSuccess(Action<TurnContext, TValue> onSuccess)
            {
                _ConversationTopicValue.OnSuccess = onSuccess;
                return this;
            }

            public ConversationTopicValueFluentInterface OnFailure(Action<TurnContext, string> onFailure)
            {
                _ConversationTopicValue.OnFailure = onFailure;
                return this;
            }
        }
    }
}