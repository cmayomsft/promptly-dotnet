using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace PromptlyBot
{
    public abstract class Topic
    {
        public abstract Task OnReceiveActivity(IBotContext context);

        protected Action<IBotContext> _onSuccess;
        public Topic OnSuccess(Action<IBotContext> success)
        {
            _onSuccess = success;
            return this;
        }

        protected Action<IBotContext, string> _onFailure;
        public Topic OnFailure(Action<IBotContext, string> failure)
        {
            _onFailure = failure;
            return this;
        }
    }

    public abstract class Topic<TValue> : Topic
    {
        protected new Action<IBotContext, TValue> _onSuccess;
        public Topic<TValue> OnSuccess(Action<IBotContext, TValue> success)
        {
            _onSuccess = success;
            return this;
        }
    }
}
