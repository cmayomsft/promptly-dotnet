using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace Promptly
{
    public abstract class Topic
    {
        public abstract Task OnReceiveActivity(IBotContext context);

        protected Func<IBotContext, Task> _onSuccess;
        public Topic OnSuccess(Func<IBotContext, Task> success)
        {
            _onSuccess = success;
            return this;
        }

        protected Func<IBotContext, string, Task> _onFailure;
        public Topic OnFailure(Func<IBotContext, string, Task> failure)
        {
            _onFailure = failure;
            return this;
        }
    }

    public abstract class Topic<TValue> : Topic
    {
        protected new Func<IBotContext, TValue, Task> _onSuccess;
        public Topic OnSuccess(Func<IBotContext, TValue, Task> success)
        {
            _onSuccess = success;
            return this;
        }
    }
}
