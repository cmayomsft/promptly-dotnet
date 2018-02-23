using Microsoft.Bot.Builder;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class SimpleValueConversationTopic : ConversationTopic<string>
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            context.Reply("SimpleConversationTopic.OnReceiveActivity()");
            return Task.CompletedTask;
        }
    }
}
