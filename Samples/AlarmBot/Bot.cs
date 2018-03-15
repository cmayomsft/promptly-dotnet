using System.Collections.Generic;
using System.Threading.Tasks;
using AlarmBot.Models;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using PromptlyBot;

namespace AlarmBot
{
    public class ConversationState : TopicsRootState
    {
    }

    public class UserState: StoreItem
    {
        public List<Alarm> Alarms { get; set; }
    }

    public class Bot : IBot
    {
        public Task OnReceiveActivity(IBotContext context)
        {
            var rootTopic = new Topics.RootTopic(context);

            rootTopic.OnReceiveActivity(context);

            return Task.CompletedTask;
        }
    }
}
