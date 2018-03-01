using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace PromptlyBot
{
    /// <summary>
    /// Interface that defines the based functionality that all conversation topics must implement.
    /// </summary>
    public interface ITopic
    {
        /// <value>
        /// Provides access to the converation topic's state for serialization between turns.
        /// </value>
        /// <remarks>
        /// Type is <c>object</c> so interface can provide access to implementing class state regardless of type.
        /// </remarks>
        object State { get; set; }

        /// <summary>
        /// Method that receives the context of the current turn and provides the appropriate response.
        /// </summary>
        /// <param name="context">An <c>IBotContext</c> that represents the current conversational turn.</param>
        /// <returns><c>Task</c> that represents an asyncronous operation.</returns>
        /// <remarks>
        /// Called on each turn when the ITopic is the active topic of conversation.
        /// </remarks>
        Task OnReceiveActivity(IBotContext context);

        /// <value>
        /// Gets/Sets the delegate to be called when the conversation topic completes successfully.
        /// </value>
        Action<IBotContext> OnSuccess { get; set; }

        /// <value>
        /// Gets/Sets the delegate to be called when the conversation topic completes successfully.
        /// </value>
        Action<IBotContext, string> OnFailure { get; set; }
    }

    /// <summary>
    /// Represents a conversation topic that doesn't return a value when it completes successfully.
    /// </summary>
    /// <typeparam name="TState">
    /// The state of the conversation topic that will be persisted between conversation turns.
    /// </typeparam>
    public abstract class Topic<TState> : ITopic where TState : new()
    {
        private readonly TopicFluentInterface _set;

        /// <summary>
        /// Constructor that establishes/initializes state and the fluent API interface.
        /// </summary>
        public Topic() : base()
        {
            this._state = new TState();

            this._set = new TopicFluentInterface(this);
        }

        /// <value>
        /// Readonly access to the <c>Topic</c>'s fluent interface.
        /// </value>
        public TopicFluentInterface Set { get => _set; }

        /// <value>
        /// Protected access to state for the fluent API interface class.
        /// </value>
        protected TState _state;
        /// <value>
        /// The <c>ITopic</c>'s state property as object.
        /// </value>
        object ITopic.State { get => _state; set => State = (TState)value; }
        /// <value>
        /// The state of the topic of conversation as <c>TState</c>.
        /// </value>
        public TState State { get => _state; set => _state = value; }


        /// <summary>
        /// See <see cref = "ITopic.OnReceiveActivity(IBotContext)"/> for more details. 
        /// </summary>
        public abstract Task OnReceiveActivity(IBotContext context);

        private Action<IBotContext> _onSuccess;
        /// <summary>
        /// See <see cref = "ITopic.OnSuccess"/> for more details. 
        /// </summary>
        public Action<IBotContext> OnSuccess { get => _onSuccess; set => _onSuccess = value; }

        private Action<IBotContext, string> _onFailure;
        /// <summary>
        /// See <see cref = "ITopic.OnFailure"/> for more details. 
        /// </summary>
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
