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
        public override Task OnReceiveActivity(IBotContext context)
        {
            context.Reply($"SimpleTopic.OnReceiveActivity() - { this._state.turns }");

            this._state.turns += 1;

            if (this._state.turns > 1)
            {
                this.OnSuccessValue(context, this._state.turns);
            }

            return Task.CompletedTask;
        }
    }
}
