using Microsoft.Bot.Builder;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class SimpleTopicState
    {
        public int turns;
    }

    public class SimpleTopic : Topic<SimpleTopicState>
    {
        public SimpleTopic(SimpleTopicState state = null) : base(state) { }

        public override Task OnReceiveActivity(IBotContext context)
        {
            context.Reply("SimpleTopic.OnReceiveActivity()");

            this.OnFailure(context, "failed");

            return Task.CompletedTask;
        }
    }
}
