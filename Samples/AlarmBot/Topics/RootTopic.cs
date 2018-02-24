using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class RootTopic : TopicsRoot
    {
        public RootTopic(IBotContext context) : base(context)
        {
            this.SubTopics.Add("simpleTopic", () =>
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

            this.SubTopics.Add("simpleValueTopic", () =>
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

            this.SubTopics.Add("addAlarmTopic", () =>
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
                    this.SetActiveTopic("simpleTopic");
                    this.ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "simple value")
                {
                    this.SetActiveTopic("simpleValueTopic");
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
                    this.SetActiveTopic("addAlarmTopic");
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
