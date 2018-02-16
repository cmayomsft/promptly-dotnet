using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace Promptly
{
    // Topic - Abstract base class for modeling a topic of conversation.
    //  S = Interface for the state of the Topic, used to manage the Topic on each turn, 
    //      or call to onReceive().
    //  V = Interface for the resulting value for when the Topic completes successfully.
    //      Optional for cases where the Topic doesn't need to return a value. 
    // TODO: Make V optional generic type.
    public abstract class Topic<TState, TValue>
    {
        private TState state;

        public Topic(TState state)
        {
            this.State = state;
        }

        public TState State { get => state; set => state = value; }

        // onReceive - Called on each turn when Topic is the active topic of conversation.
        protected abstract Task OnReceiveActivity(IBotContext context);

        // onSuccess - Function to call when the Topic completes successfully, passing the
        //  resulting value of the Topic.
        protected Func<IBotContext, V, Task> success;
        public Topic<TState, V> OnSuccess(Func<IBotContext, V, Task> success)
        {
            this.success = success;
            return this;
        }

        // onFailure - Function to call when the Topic ends unsuccessfully, passing the reason
        //  why the Topic failed. 
        protected Func<IBotContext, string, Task> failure;
        public Topic<TState, V> OnFailure(Func<IBotContext, string, Task> failure)
        {
            this.failure = failure;
            return this;
        }
    }
}
