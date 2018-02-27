using Topics.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Topics.Topics
{
    public class RootTopic : TopicsRoot
    {
        private const string ADD_ALARM_TOPIC = "addAlarmTopic";

        private const string USER_STATE_ALARMS = "Alarms";

        public RootTopic(IBotContext context) : base(context)
        {
            // User state initialization should be done once in the welcome 
            //  new user feature. Placing it here until that feature is added.
            if (context.State.UserProperties[USER_STATE_ALARMS] == null)
            {
                context.State.UserProperties[USER_STATE_ALARMS] = new List<Alarm>();
            }

            this.SubTopics.Add(ADD_ALARM_TOPIC, () =>
            {
                var addAlarmTopic = new AddAlarmTopic();

                addAlarmTopic.Set
                    .OnSuccess((ctx, alarm) =>
                    {
                        this.ClearActiveTopic();

                        ((List<Alarm>)ctx.State.UserProperties[USER_STATE_ALARMS]).Add(alarm);

                        context.Reply($"Added alarm named '{ alarm.Title }' set for '{ alarm.Time }'.");
                    })
                    .OnFailure((ctx, reason) =>
                    {
                        this.ClearActiveTopic();

                        context.Reply("Let's try something else.");

                        this.ShowDefaultMessage(ctx);
                    });

                return addAlarmTopic;
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

                // If there is an active topic, let it handle this turn until it completes.
                if (HasActiveTopic)
                {
                    this.ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                ShowDefaultMessage(context);
            }

            return Task.CompletedTask;
        }

        private void ShowDefaultMessage(IBotContext context)
        {
            context.Reply("'Add Alarm'.");
        }
    }
}




            /*
            this.SubTopics.Add(ADD_ALARM_TOPIC, () =>
            {
                var addAlarmTopic = new AddAlarmTopic();

                addAlarmTopic.Set
                    .OnSuccess((ctx, alarm) =>
                        {
                            this.ClearActiveTopic();

                            ((List<Alarm>)ctx.State.UserProperties[USER_STATE_ALARMS]).Add(alarm);

                            context.Reply($"Added alarm named '{ alarm.Title }' set for '{ alarm.Time }'.");
                        })
                    .OnFailure((ctx, reason) =>
                        {
                            this.ClearActiveTopic();

                            context.Reply("Let's try something else.");
                            
                            this.ShowDefaultMessage(ctx);
                        });

                return addAlarmTopic;
            });
            */
                    

                    /*
                    this.SetActiveTopic(ADD_ALARM_TOPIC)
                            .OnReceiveActivity(context);
                    return Task.CompletedTask;
                    */

                /*
                if (HasActiveTopic)
                {
                    ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }
                */

