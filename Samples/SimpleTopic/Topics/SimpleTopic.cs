using Microsoft.Bot.Builder;
using System;
using System.Threading.Tasks;

namespace SimpleTopics.Topics
{
    public class SimpleTopic
    {
        public int Turns { get; set; }

        // TODO: Why is this not called OnReceive() like Node? What else can it receive?
        public Task OnReceiveActivity(IBotContext context)
        {
            context.Reply($"SimpleTopic.OnReceiveActivity() - Turn: { this.Turns }");

            this.Turns += 1;

            return Task.CompletedTask;
        }
    }
}
