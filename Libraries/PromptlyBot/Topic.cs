using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace PromptlyBot
{
    public abstract class Topic
    {
        public abstract Task OnReceiveActivity(IBotContext context);

        internal Action<IBotContext> _onSuccess;

        protected Action<IBotContext, string> _onFailure;
        public Action<IBotContext, string> OnFailure
        {
            set { _onFailure = value; }
            get { return _onFailure; }
        }
    }

    public static class TopicExtension
    {
        public static T OnSuccess<T>(this T topic, Action<IBotContext> onSuccess) where T : Topic
        {
            topic._onSuccess = onSuccess;
            return topic;
        }
    }

    public abstract class Topic<TValue> : Topic
    { 
        new internal Action<IBotContext, TValue> _onSuccess;
    }

    public static class TopicTValueExtension
    {
        public static T OnSuccess<T, V>(this T topic, Action<IBotContext, V> onSuccess) where T: Topic<V>
        {
            topic._onSuccess = onSuccess;
            return topic;
        }
    }
}
