using Microsoft.Bot.Builder;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class SimpleValueTopicState
    {
        public int turns;
    }

    public class SimpleValueTopic : Topic<SimpleTopicState, int>
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            context.Reply($"SimpleValueTopic.OnReceiveActivity() - { this._state.turns }");

            this._state.turns += 1;

            if (this._state.turns > 1)
            {
                this.OnSuccess(context, this.State.turns);
            }

            return Task.CompletedTask;
        }
    }
}