using Microsoft.Bot;
using PromptlyBot;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using SimplePrompt.Topics;

namespace SimplePrompt
{
    public class BotConversationState : PromptlyBotConversationState<RootTopicState>
    {
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
