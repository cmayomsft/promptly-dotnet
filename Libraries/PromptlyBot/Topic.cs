using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace PromptlyBot
{
    public abstract class Topic<TValue>
    {
        public abstract Task OnReceiveActivity(IBotContext context);

        protected Action<IBotContext, TValue> _onSuccess;
        public Topic<TValue> OnSuccess(Action<IBotContext, TValue> success)
        {
            _onSuccess = success;
            return this;
        }

        protected Action<IBotContext, string> _onFailure;
        public Topic<TValue> OnFailure(Action<IBotContext, string> failure)
        {
            _onFailure = failure;
            return this;
        }
    }
}
