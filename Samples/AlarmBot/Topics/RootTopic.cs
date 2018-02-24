using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class RootTopic : TopicsRoot
    {
        private const string SIMPLE_TOPIC = "simpleTopic";
        private const string SIMPLE_VALUE_TOPIC = "simpleValueTopic";
        private const string ADD_ALARM_TOPIC = "addAlarmTopic";

        public RootTopic(IBotContext context) : base(context)
        {
            this.SubTopics.Add(SIMPLE_TOPIC, () =>
                {
                    return new SimpleTopic
                    {
                        OnSuccess = (ctx) =>
                            {
                                context.Reply($"SimpleTopic.OnSuccess()");
                                this.ClearActiveTopic();
                            },
                        OnFailure = (ctx, reason) =>
                            {
                                this.ClearActiveTopic();
                                context.Reply($"SimpleTopic.OnFailure() - { reason }");
                            },

                    };
                });

            this.SubTopics.Add(SIMPLE_VALUE_TOPIC, () =>
            {
                return new SimpleValueTopic
                {
                    OnSuccess = (ctx, value) =>
                    {
                        context.Reply($"SimpleValueTopic.OnSuccess() - { value }");
                        this.ClearActiveTopic();
                    },
                    OnFailure = (ctx, reason) =>
                    {
                        this.ClearActiveTopic();
                        context.Reply($"SimpleValueTopic.OnFailure() - { reason }");
                    },

                };
            });

            this.SubTopics.Add(ADD_ALARM_TOPIC, () =>
            {
                return new AddAlarmTopic
                {
                    OnSuccess = (ctx, value) =>
                    {
                        context.Reply($"AddAlarmTopic.OnSuccess() - { value.Title + " - " + value.Time }");
                        this.ClearActiveTopic();
                    },
                    OnFailure = (ctx, reason) =>
                    {
                        this.ClearActiveTopic();
                        context.Reply($"AddAlarmTopic.OnFailure() - { reason }");
                    },

                };
            });
        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity();

                if (message.Text.ToLowerInvariant() == "simple")
                {
                    this.SetActiveTopic(SIMPLE_TOPIC);
                    this.ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "simple value")
                {
                    this.SetActiveTopic(SIMPLE_VALUE_TOPIC);
                    this.ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                /*if (message.Text.ToLowerInvariant() == "simple conversation topic")
                {
                    var simpleConversationTopic = new SimpleConversationTopic();
                    simpleConversationTopic.OnSuccess = (ctx) => { };
                    simpleConversationTopic.OnFailure = (ctx, reason) => { };

                    this.ActiveTopic = simpleConversationTopic;

                    simpleConversationTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }*/

                /*if (message.Text.ToLowerInvariant() == "simple value conversation topic")
                {
                    var simpleValueConversationTopic = new SimpleValueConversationTopic();
                    simpleValueConversationTopic.OnSuccess = (ctx, value) => { };
                    simpleValueConversationTopic.OnFailure = (ctx, reason) => { };

                    this.ActiveTopic = simpleValueConversationTopic;

                    simpleValueConversationTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }*/


                if (message.Text.ToLowerInvariant() == "add alarm")
                {
                    this.SetActiveTopic(ADD_ALARM_TOPIC);
                    this.ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                if (HasActiveTopic)
                {
                    ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                ShowDefaultMessage(context);
            }

            return Task.CompletedTask;
        }

        private void ShowDefaultMessage(IBotContext context)
        {
            context.Reply("'Show Alarms', 'Add Alarm', 'Delete Alarm', 'Help'.");
        }
    }
}
