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
    public class RootTopicState : ConversationTopicState
    {

    }

    public class RootTopic : TopicsRoot<BotConversationState, RootTopicState>
    {
        private const string ADD_ALARM_TOPIC = "addAlarmTopic";
        private const string DELETE_ALARM_TOPIC = "deleteAlarmTopic";

        public RootTopic(ITurnContext turnContext) : base(turnContext)
        {
            // User state initialization should be done once in the welcome 
            //  new user feature. Placing it here until that feature is added.
            if (turnContext.GetUserState<BotUserState>().Alarms == null)
            {
                turnContext.GetUserState<BotUserState>().Alarms = new List<Alarm>();
            }

            this.SubTopics.Add(ADD_ALARM_TOPIC, (object[] args) =>
            {
                var addAlarmTopic = new AddAlarmTopic();

                addAlarmTopic.Set
                    .OnSuccess((turn, alarm) =>
                        {
                            ClearActiveTopic();

                            turn.GetUserState<BotUserState>().Alarms.Add(alarm);

                            turn.SendActivity($"Added alarm named '{ alarm.Title }' set for '{ alarm.Time }'.");
                        })
                    .OnFailure((turn, reason) =>
                        {
                            ClearActiveTopic();

                            turn.SendActivity("Let's try something else.");
                            
                            ShowDefaultMessage(turnContext);
                        });

                return addAlarmTopic;
            });

            this.SubTopics.Add(DELETE_ALARM_TOPIC, (object[] args) =>
            {
                var alarms = (args.Length > 0) ? (List<Alarm>)args[0] : null;

                var deleteAlarmTopic = new DeleteAlarmTopic(alarms);

                deleteAlarmTopic.Set
                    .OnSuccess((turn, value) =>
                        {
                            this.ClearActiveTopic();

                            if (!value.DeleteConfirmed)
                            {
                                turn.SendActivity($"Ok, I won't delete alarm '{ value.Alarm.Title }'.");
                                return;
                            }

                            turn.GetUserState<BotUserState>().Alarms.RemoveAt(value.AlarmIndex);

                            turn.SendActivity($"Done. I've deleted alarm '{ value.Alarm.Title }'.");
                        })
                    .OnFailure((turn, reason) =>
                        {
                            ClearActiveTopic();

                            turn.SendActivity("Let's try something else.");

                            ShowDefaultMessage(turn);
                        });

                return deleteAlarmTopic;
            });
        }

        public override Task OnTurn(ITurnContext turnContext)
        {
            if ((turnContext.Activity.Type == ActivityTypes.Message) && (turnContext.Activity.Text.Length > 0))
            {
                var message = turnContext.Activity;

                // If the user wants to change the topic of conversation...
                if (message.Text.ToLowerInvariant() == "add alarm")
                {
                    // Set the active topic and let the active topic handle this turn.
                    return SetActiveTopic(ADD_ALARM_TOPIC)
                            .OnTurn(turnContext);
                }

                if (message.Text.ToLowerInvariant() == "delete alarm")
                {
                    return SetActiveTopic(DELETE_ALARM_TOPIC, turnContext.GetUserState<BotUserState>().Alarms)
                        .OnTurn(turnContext);
                }

                if (message.Text.ToLowerInvariant() == "show alarms")
                {
                    ClearActiveTopic();

                    AlarmsView.ShowAlarms(turnContext, turnContext.GetUserState<BotUserState>().Alarms);
                    return Task.CompletedTask;
                }

                if (message.Text.ToLowerInvariant() == "help")
                {
                    ClearActiveTopic();

                    return ShowHelp(turnContext);
                }

                // If there is an active topic, let it handle this turn until it completes.
                if (HasActiveTopic)
                {
                    return ActiveTopic.OnTurn(turnContext);
                }

                return ShowDefaultMessage(turnContext);
            }

            return Task.CompletedTask;
        }

        private Task ShowDefaultMessage(ITurnContext turnContext)
        {
            return turnContext.SendActivity("'Show Alarms', 'Add Alarm', 'Delete Alarm', 'Help'.");
        }

        private Task ShowHelp(ITurnContext turnContext)
        {
            var message = "Here's what I can do:\n\n";
            message += "To see your alarms, say 'Show Alarms'.\n\n";
            message += "To add an alarm, say 'Add Alarm'.\n\n";
            message += "To delete an alarm, say 'Delete Alarm'.\n\n";
            message += "To see this again, say 'Help'.";

            return turnContext.SendActivity(message);
        }
    }
}
