using Microsoft.Bot.Builder;
using PromptlyBot;
using System;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    [Serializable]
    public class SimpleTopic : Topic
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            context.Reply("SimpleTopic.OnReceiveActivity()");
            return Task.CompletedTask;
        }
    }
}
