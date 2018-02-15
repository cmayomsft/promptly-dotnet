using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmBot.Promptly
{
    // Topic - Abstract base class for modeling a topic of conversation.
    //  S = Interface for the state of the Topic, used to manage the Topic on each turn, 
    //      or call to onReceive().
    //  V = Interface for the resulting value for when the Topic completes successfully.
    //      Optional for cases where the Topic doesn't need to return a value. 
    public abstract class Topic<S, V>
    {
        private S state;

        public Topic(S state)
        {
            this.State = state;
        }

        public S State { get => state; set => state = value; }

        // onReceive - Called on each turn when Topic is the active topic of conversation.
        protected abstract Task OnReceiveActivity(IBotContext context);

        // onSuccess - Function to call when the Topic completes successfully, passing the
        //  resulting value of the Topic.
        protected Func<IBotContext, V, Task> success;
        public Topic<S, V> OnSuccess(Func<IBotContext, V, Task> success)
        {
            this.success = success;
            return this;
        }

        // onFailure - Function to call when the Topic ends unsuccessfully, passing the reason
        //  why the Topic failed. 
        protected Func<IBotContext, V, Task> failure;
        public Topic<S, V> OnFailure(Func<IBotContext, V, Task> failure)
        {
            this.failure = failure;
            return this;
        }
    }
}
