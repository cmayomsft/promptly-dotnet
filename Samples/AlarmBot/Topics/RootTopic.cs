using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PromptlyBot;
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

                if (message.Text.ToLowerInvariant() == "simple")
                {
                    var simpleTopic = new SimpleTopic()
                        .SetOnSuccess(((ctx) => { }))
                        .SetOnFailure((ctx, reason) => { });

                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "simple value")
                {
                    var simpleValueTopic = new SimpleValueTopic()
                        .SetOnSuccess((ctx, value) => { });

                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "simple conversation topic")
                {
                    var simpleTopic = new SimpleConversationTopic()
                        .SetOnSuccess(((ctx) => { }))
                        .SetOnFailure((ctx, reason) => { });

                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "simple value conversation topic")
                {
                    var simpleTopic = new SimpleConversationTopic()
                        .SetOnSuccess(((ctx, value) => { }))
                        .SetOnFailure((ctx, reason) => { });

                    return Task.CompletedTask;
                }


                if (message.Text.ToLowerInvariant() == "add alarm")
                {
                    var addAlarmTopic = new AddAlarmTopic();

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
