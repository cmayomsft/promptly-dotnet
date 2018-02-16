using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace Promptly
{
    public abstract class Topic
    {
        // onReceive - Called on each turn when Topic is the active topic of conversation.
        protected abstract Task OnReceiveActivity(IBotContext context);

        // onSuccess - Function to call when the Topic completes successfully, passing the
        //  resulting value of the Topic.
        protected Func<IBotContext, Task> success;
        public Topic OnSuccess(Func<IBotContext, Task> success)
        {
            this.success = success;
            return this;
        }

        // onFailure - Function to call when the Topic ends unsuccessfully, passing the reason
        //  why the Topic failed. 
        protected Func<IBotContext, string, Task> failure;
        public Topic OnFailure(Func<IBotContext, string, Task> failure)
        {
            this.failure = failure;
            return this;
        }
    }

    public abstract class Topic<TValue> : Topic
    {
        // onSuccess - Function to call when the Topic completes successfully, passing the
        //  resulting value of the Topic.
        protected new Func<IBotContext, TValue, Task> success;
        public Topic OnSuccess(Func<IBotContext, TValue, Task> success)
        {
            this.success = success;
            return this;
        }
    }
}
