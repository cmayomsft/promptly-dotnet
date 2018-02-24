using AlarmBot.Models;
using Microsoft.Bot.Builder;
using PromptlyBot;
using PromptlyBot.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class DeleteAlarmTopicState : ConversationTopicState
    {
        public List<Alarm> Alarms;
        public int AlarmIndex;
        public Alarm Alarm;
        public bool DeleteConfirmed;
    }

    public class DeleteAlarmTopicValue
    {
        public int AlarmIndex;
        public Alarm Alarm;
        public bool DeleteConfirmed;
    }

    public class DeleteAlarmTopic : ConversationTopic<DeleteAlarmTopicState, DeleteAlarmTopicValue>
    {
        private const string WHICH_ALARM_PROMPT = "whichAlarmPrompt";
        private const string CONFIRM_DELETE_PROMPT = "confirmDeletePrompt";

        public DeleteAlarmTopic(List<Alarm> alarms) : base()    
        {
            if (alarms != null)
            {
                this._state.Alarms = alarms;
            }

            this.SubTopics.Add(WHICH_ALARM_PROMPT, () =>
            {
                return new Prompt<int>
                {
                    OnPrompt = (context, lastTurnReason) =>
                    {
                        if ((lastTurnReason != null) && (lastTurnReason == "indexnotfound"))
                        {
                            context.Reply("Sorry, alarm titles must be less that 20 characters.")
                                .Reply("Let's try again.");
                        }

                        this.ShowAlarms(context, this._state.Alarms);

                        context.Reply("Which alarm would you like to delete?");
                    },
                    Validator = new AlarmTitleValidator(),
                    MaxTurns = 2,
                    OnSuccess = (context, value) =>
                    {
                        this.ClearActiveTopic();

                        this.State.alarm.Title = value;

                        this.OnReceiveActivity(context);
                    },
                    OnFailure = (context, reason) =>
                    {
                        this.ClearActiveTopic();

                        if ((reason != null) && (reason == "toomanyattempts"))
                        {
                            context.Reply("I'm sorry I'm having issues understanding you.");
                        }
                    }
                };
            });

            this.SubTopics.Add(TIME_PROMPT, () =>
            {
                return new Prompt<string>
                {
                    OnPrompt = (context, lastTurnReason) =>
                    {
                        context.Reply("What time would you like to set your alarm for?");
                    },
                    Validator = new AlarmTimeValidator(),
                    MaxTurns = 2,
                    OnSuccess = (context, value) =>
                    {
                        this.ClearActiveTopic();

                        this.State.alarm.Time = value;

                        this.OnReceiveActivity(context);
                    },
                    OnFailure = (context, reason) =>
                    {
                        this.ClearActiveTopic();

                        if ((reason != null) && (reason == "toomanyattempts"))
                        {
                            context.Reply("I'm sorry I'm having issues understanding you.");
                        }
                    }
                };
            });

        }

        private void ShowAlarms(IBotContext context, List<Alarm> alarms)
        {
            if ((alarms == null) || (alarms.Count == 0))
            {
                context.Reply("You have no alarms.");
                return;
            }

            if (alarms.Count == 1)
            {
                context.Reply($"You have one alarm named '{ alarms[0].Time }', set for '{ alarms[0].Time }'.");
                return;
            }

            string message = $"You have { alarms.Count } alarms: \n\n";
            foreach (var alarm in alarms)
            {
                message += $"'{ alarm.Title }' set for '{ alarm.Time }' \n\n";
            }

            context.Reply(message);
        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class AlarmIndexValidator : Validator<int>
    {
        private List<Alarm> _alarms;

        public AlarmIndexValidator(List<Alarm> alarms) : base()
        {
            _alarms = alarms;
        }

        public override ValidatorResult<int> Validate(IBotContext context)
        {
            int index = this._alarms.FindIndex(alarm => alarm.Title.ToLowerInvariant() == context.Request.AsMessageActivity().Text.ToLowerInvariant());

            if (index > -1)
            {
                return new ValidatorResult<int>
                {
                    Value = index
                };
            }
            else
            {
                return new ValidatorResult<int>
                {
                    Reason = "indexnotfound"
                };
            }
        }
    }
}
