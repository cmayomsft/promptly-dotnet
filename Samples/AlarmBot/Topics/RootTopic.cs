using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class RootTopic : ConversationTopic<string>
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity();

                // Start Here: Wire up a simple Topic and Topic<> with onSuccess/onFailure.
                if (message.Text.ToLowerInvariant() == "add alarm")
                {
                    ActiveTopic = new AddAlarmTopic();

                    ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                if (HasActiveTopic)
                {
                    ActiveTopic.OnReceiveActivity(context);
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
