using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace PromptlyBot
{
    public abstract class Topic<TValue>
    {
        public abstract Task OnReceiveActivity(IBotContext context);

        protected Action<IBotContext, TValue> _onSuccess;
        public Action<IBotContext, TValue> OnSuccess
        {
            set { _onSuccess = value; }
            get { return _onSuccess; }
        }

        protected Action<IBotContext, string> _onFailure;
        public Action<IBotContext, string> OnFailure
        {
            set { _onFailure = value; }
            get { return _onFailure; }
        }
    }

    public static class TopicTValueExtension
    {
        public static T OnSuccess<T, V>(this T topic, Action<IBotContext, V> onSuccess) where T: Topic<V>
        {
            topic.OnSuccess = onSuccess;
            return topic;
        }
    }
}
