using AlarmBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class RootTopic : TopicsRoot
    {
        private const string ADD_ALARM_TOPIC = "addAlarmTopic";
        private const string DELETE_ALARM_TOPIC = "deleteAlarmTopic";

        private const string USER_STATE_ALARMS = "Alarms";

        public RootTopic(IBotContext context) : base(context)
        {
            // User state initialization should be done once in the welcome 
            //  new user feature. Placing it here until that feature is added.
            if (context.State.User[USER_STATE_ALARMS] == null)
            {
                context.State.User[USER_STATE_ALARMS] = new List<Alarm>();
            }

            this.SubTopics.Add(ADD_ALARM_TOPIC, () =>
            {
                return new AddAlarmTopic
                {
                    OnSuccess = (ctx, alarm) =>
                    {
                        this.ClearActiveTopic();

                        ((List<Alarm>)ctx.State.User[USER_STATE_ALARMS]).Add(alarm);

                        context.Reply($"Added alarm named '{ alarm.Title }' set for '{ alarm.Time }'.");
                    },
                    OnFailure = (ctx, reason) =>
                    {
                        this.ClearActiveTopic();

                        context.Reply("Let's try something else.");

                        this.ShowDefaultMessage(context);
                    }
                };
            });

            this.SubTopics.Add(DELETE_ALARM_TOPIC, () =>
            {
                return new DeleteAlarmTopic(context.State.User[USER_STATE_ALARMS])
                {
                    OnSuccess = (ctx, value) =>
                    {
                        this.ClearActiveTopic();

                        if (!value.DeleteConfirmed)
                        {
                            context.Reply($"Ok, I won't delete alarm '{ value.Alarm.Title }'.");
                            return;
                        }

                        ((List<Alarm>)ctx.State.User[USER_STATE_ALARMS]).RemoveAt(value.AlarmIndex);

                        context.Reply($"Done. I've deleted alarm '{ value.Alarm.Title }'.");
                    },
                    OnFailure = (ctx, reason) =>
                    {
                        this.ClearActiveTopic();

                        context.Reply("Let's try something else.");

                        this.ShowDefaultMessage(context);
                    }
                };
            });
        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity();

                if (message.Text.ToLowerInvariant() == "add alarm")
                {
                    this.SetActiveTopic(ADD_ALARM_TOPIC);
                    this.ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "delete alarm")
                {
                    this.SetActiveTopic(DELETE_ALARM_TOPIC);
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
