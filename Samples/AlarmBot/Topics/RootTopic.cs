using AlarmBot.Models;
using AlarmBot.Views;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class RootTopic : TopicsRoot<BotConversationState>
    {
        private const string ADD_ALARM_TOPIC = "addAlarmTopic";
        private const string DELETE_ALARM_TOPIC = "deleteAlarmTopic";

        public RootTopic(IBotContext context) : base(context)
        {
            // User state initialization should be done once in the welcome 
            //  new user feature. Placing it here until that feature is added.
            if (context.GetUserState<BotUserState>().Alarms == null)
            {
                context.GetUserState<BotUserState>().Alarms = new List<Alarm>();
            }

            this.SubTopics.Add(ADD_ALARM_TOPIC, (object[] args) =>
            {
                var addAlarmTopic = new AddAlarmTopic();

                addAlarmTopic.Set
                    .OnSuccess((ctx, alarm) =>
                        {
                            this.ClearActiveTopic();

                            ctx.GetUserState<BotUserState>().Alarms.Add(alarm);

                            context.SendActivity($"Added alarm named '{ alarm.Title }' set for '{ alarm.Time }'.");
                        })
                    .OnFailure((ctx, reason) =>
                        {
                            this.ClearActiveTopic();

                            context.SendActivity("Let's try something else.");
                            
                            this.ShowDefaultMessage(ctx);
                        });

                return addAlarmTopic;
            });

            this.SubTopics.Add(DELETE_ALARM_TOPIC, (object[] args) =>
            {
                var alarms = (args.Length > 0) ? (List<Alarm>)args[0] : null;

                var deleteAlarmTopic = new DeleteAlarmTopic(alarms);

                deleteAlarmTopic.Set
                    .OnSuccess((ctx, value) =>
                        {
                            this.ClearActiveTopic();

                            if (!value.DeleteConfirmed)
                            {
                                context.SendActivity($"Ok, I won't delete alarm '{ value.Alarm.Title }'.");
                                return;
                            }

                            ctx.GetUserState<BotUserState>().Alarms.RemoveAt(value.AlarmIndex);

                            context.SendActivity($"Done. I've deleted alarm '{ value.Alarm.Title }'.");
                        })
                    .OnFailure((ctx, reason) =>
                        {
                            this.ClearActiveTopic();

                            context.SendActivity("Let's try something else.");

                            this.ShowDefaultMessage(context);
                        });

                return deleteAlarmTopic;
            });
        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity();

                // If the user wants to change the topic of conversation...
                if (message.Text.ToLowerInvariant() == "add alarm")
                {
                    // Set the active topic and let the active topic handle this turn.
                    this.SetActiveTopic(ADD_ALARM_TOPIC)
                            .OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "delete alarm")
                {
                    this.SetActiveTopic(DELETE_ALARM_TOPIC, context.GetUserState<BotUserState>().Alarms)
                        .OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "show alarms")
                {
                    this.ClearActiveTopic();

                    AlarmsView.ShowAlarms(context, context.GetUserState<BotUserState>().Alarms);
                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "help")
                {
                    this.ClearActiveTopic();

                    this.ShowHelp(context);
                    return Task.CompletedTask;
                }

                // If there is an active topic, let it handle this turn until it completes.
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
            context.SendActivity("'Show Alarms', 'Add Alarm', 'Delete Alarm', 'Help'.");
        }

        private void ShowHelp(IBotContext context)
        {
            var message = "Here's what I can do:\n\n";
            message += "To see your alarms, say 'Show Alarms'.\n\n";
            message += "To add an alarm, say 'Add Alarm'.\n\n";
            message += "To delete an alarm, say 'Delete Alarm'.\n\n";
            message += "To see this again, say 'Help'.";

            context.SendActivity(message);
        }
    }
}
