using System.Collections.Generic;
using System.Threading.Tasks;
using AlarmBot.Models;
using AlarmBot.Topics;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using PromptlyBot;

namespace AlarmBot
{
    public class BotConversationState : PromptlyBotConversationState<RootTopicState>
    {
    }

    public class BotUserState
    {
        public List<Alarm> Alarms { get; set; }
    }

    public class Bot : IBot
    {
        public Task OnTurn(ITurnContext turnContext)
        {
            var rootTopic = new Topics.RootTopic(turnContext);

            return rootTopic.OnTurn(turnContext);
        }
    }
}
