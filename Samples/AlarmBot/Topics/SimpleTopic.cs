using Microsoft.Bot.Builder;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class SimpleTopic : Topic
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            context.Reply("SimpleTopic.OnReceiveActivity()");
            return Task.CompletedTask;
        }
    }
}
