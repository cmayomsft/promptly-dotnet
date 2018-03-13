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
    /// Abstract class that represents a conversation topic that doesn't return a value when it completes successfully.
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
        /// Protected access to state for the fluent API class.
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
        /// <value>
        /// See <see cref = "ITopic.OnSuccess"/> for more details. 
        /// </value>
        public Action<IBotContext> OnSuccess { get => _onSuccess; set => _onSuccess = value; }

        private Action<IBotContext, string> _onFailure;
        /// <summary>
        /// See <see cref = "ITopic.OnFailure"/> for more details. 
        /// </summary>
        public Action<IBotContext, string> OnFailure { get => _onFailure; set => _onFailure = value; }
        
        /// <summary>
        /// Internal class to define the fluent API for <see cref="Topic{TState}"/>.
        /// </summary>
        public class TopicFluentInterface
        {
            private readonly Topic<TState> _topic;

            /// <summary>
            /// Establishes reference between Topic and this fluent API class.
            /// </summary>
            /// <param name="topic">The <see cref="Topic{TState}"/> referenced in the fluent API.</param>
            public TopicFluentInterface(Topic<TState> topic)
            {
                this._topic = topic;
            }

            /// <summary>
            /// Fluent method for setting <see cref="ITopic.OnSuccess"/>/>.
            /// </summary>
            /// <param name="onSuccess">See <see cref="ITopic.OnSuccess"/>.</param>
            /// <returns>A reference to <c>this</c> for fluent API calls.</returns>
            public TopicFluentInterface OnSuccess(Action<IBotContext> onSuccess)
            {
                _topic._onSuccess = onSuccess;
                return this;
            }

            /// <summary>
            /// Fluent method for setting <see cref="ITopic.OnFailure"/>/>.
            /// </summary>
            /// <param name="onFailure">See <see cref="ITopic.OnFailure"/>.</param>
            /// <returns>A reference to <c>this</c> for fluent API calls.</returns>
            public TopicFluentInterface OnFailure(Action<IBotContext, string> onFailure)
            {
                _topic._onFailure = onFailure;
                return this;
            }
        }
    }

    /// <summary>
    /// Abstract class that represents a conversation topic that returns a value when it completes successfully.
    /// </summary>
    /// <typeparam name="TState">
    /// The state of the conversation topic that will be persisted between conversation turns.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The value that will be returned when the <c>Topic</c> completes successfully.
    /// </typeparam>
    public abstract class Topic<TState, TValue> : Topic<TState> where TState : new()
    {
        private readonly TopicValueFluentInterface _set;

        /// <summary>
        /// See <see cref="Topic{TState}"/>.
        /// </summary>
        public Topic() : base()
        {
            this._set = new TopicValueFluentInterface(this);
        }

        /// <value>
        /// See <see cref="Topic{TState}.Set"/>.
        /// </value>
        new public TopicValueFluentInterface Set { get => _set; }

        private Action<IBotContext, TValue> _onSuccessValue;
        /// <value>
        /// Gets/Sets the delegate to be called when the conversation topic completes successfully.
        /// </value>
        /// <remarks>Note: For <see cref="Topic{TState, TValue}"/>, this delegate acccepts param of type <c>TValue</c> so the <c>Topic</c> can return a value when it completes successfully.</remarks>
        new public Action<IBotContext, TValue> OnSuccess { get => _onSuccessValue; set => _onSuccessValue = value; }

        /// <summary>
        /// Internal class to define the fluent API for <see cref="Topic{TState, TValue}"/>.
        /// </summary>
        public class TopicValueFluentInterface
        {
            private readonly Topic<TState, TValue> _topic;

            /// <summary>
            /// Establishes reference between <see cref="Topic{TState, TValue}"/> and this fluent API class.
            /// </summary>
            /// <param name="topic">The <see cref="Topic{TState, TValue}"/> referenced in the fluent API.</param>
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
