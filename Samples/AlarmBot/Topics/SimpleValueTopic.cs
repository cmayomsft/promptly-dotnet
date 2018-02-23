using Microsoft.Bot.Builder;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class SimpleValueTopic : Topic<string>
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            this.SetOnSuccess((ctx, value) => { });
            context.Reply("SimpleValueTopic.OnReceiveActivity()");
            return Task.CompletedTask;
        }
    }
}