using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace Promptly
{
    public abstract class Topic
    {
        public abstract Task OnReceiveActivity(IBotContext context);

        protected Func<IBotContext, Task> success;
        public Topic OnSuccess(Func<IBotContext, Task> success)
        {
            this.success = success;
            return this;
        }

        protected Func<IBotContext, string, Task> failure;
        public Topic OnFailure(Func<IBotContext, string, Task> failure)
        {
            this.failure = failure;
            return this;
        }
    }

    public abstract class Topic<TValue> : Topic
    {
        protected new Func<IBotContext, TValue, Task> success;
        public Topic OnSuccess(Func<IBotContext, TValue, Task> success)
        {
            this.success = success;
            return this;
        }
    }
}
