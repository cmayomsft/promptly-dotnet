using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Promptly;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class RootTopic : ConversationTopic
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity();

                if (message.Text.ToLowerInvariant() == "add alarm")
                {

                    context.Reply("Adding an alarm...");
                    return Task.CompletedTask;
                }

                showDefaultMessage(context);
            }

            return Task.CompletedTask;
        }

        private void showDefaultMessage(IBotContext context)
        {
            context.Reply("'Show Alarms', 'Add Alarm', 'Delete Alarm', 'Help'.");
        }
    }
}
