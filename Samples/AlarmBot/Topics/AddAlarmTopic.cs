using AlarmBot.Models;
using Microsoft.Bot.Builder;
using PromptlyBot;
using System;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class AddAlarmTopic : ConversationTopic<Alarm>
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            context.Reply("AddAlarmTopic.OnReceiveActivity()");
            return Task.CompletedTask;
        }
    }
}
