using AlarmBot.Models;
using AlarmBot.Views;
using Microsoft.Bot.Builder;
using PromptlyBot;
using PromptlyBot.Validator;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class DeleteAlarmTopicState : ConversationTopicState
    {
        public List<Alarm> Alarms;
        public int? AlarmIndex;
        public Alarm Alarm = new Alarm();
        public bool? DeleteConfirmed;
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

            this.SubTopics.Add(WHICH_ALARM_PROMPT, (object[] args) =>
            {
                var whichAlarmPrompt = new Prompt<int>();

                whichAlarmPrompt.Set
                    .OnPrompt((context, lastTurnReason) =>
                    {
                        if ((lastTurnReason != null) && (lastTurnReason == "indexnotfound"))
                        {
                            context.SendActivity($"Sorry, I coulnd't find an alarm named '{ context.Request.AsMessageActivity().Text }'.", 
                                "Let's try again.");
                        }

                        AlarmsView.ShowAlarms(context, this._state.Alarms);

                        context.SendActivity("Which alarm would you like to delete?");
                    })
                    .Validator(new AlarmIndexValidator(this._state.Alarms))
                    .MaxTurns(2)
                    .OnSuccess((context, index) =>
                        {
                            this.ClearActiveTopic();

                            this.State.AlarmIndex = index;

                            this.OnReceiveActivity(context);
                        })
                    .OnFailure((context, reason) =>
                        {
                            this.ClearActiveTopic();

                            if ((reason != null) && (reason == "toomanyattempts"))
                            {
                                context.SendActivity("I'm sorry I'm having issues understanding you.");
                            }

                            this.OnFailure(context, reason);
                        });

                return whichAlarmPrompt;
            });

            this.SubTopics.Add(CONFIRM_DELETE_PROMPT, (object[] args) =>
            {
                var confirmDeletePrompt = new Prompt<bool>();

                confirmDeletePrompt.Set
                    .OnPrompt((context, lastTurnReason) =>
                    {
                        if ((lastTurnReason != null) & (lastTurnReason == "notyesorno"))
                        {
                            context.SendActivity("Sorry, I was expecting 'yes' or 'no'.",
                                "Let's try again.");
                        }

                        context.SendActivity($"Are you sure you want to delete alarm '{ this.State.Alarm.Title }' ('yes' or 'no')?`");
                    })
                    .Validator(new YesOrNoValidator())
                    .MaxTurns(2)
                    .OnSuccess((context, value) =>
                        {
                            this.ClearActiveTopic();

                            this.State.DeleteConfirmed = value;

                            this.OnReceiveActivity(context);
                        })
                    .OnFailure((context, reason) =>
                        {
                            this.ClearActiveTopic();

                            if ((reason != null) && (reason == "toomanyattempts"))
                            {
                                context.SendActivity("I'm sorry I'm having issues understanding you.");
                            }

                            this.OnFailure(context, reason);
                        });

                return confirmDeletePrompt;
            });

        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            if (HasActiveTopic)
            {
                ActiveTopic.OnReceiveActivity(context);
                return Task.CompletedTask;
            }

            // If there are no alarms to delete...
            if (this.State.Alarms.Count == 0)
            {
                context.SendActivity("There are no alarms to delete.");
                return Task.CompletedTask;
            }

            if (this.State.AlarmIndex == null)
            {
                // If there is only one alarm to delete, use that index. No need to prompt.
                if (this.State.Alarms.Count == 1)
                {
                    AlarmsView.ShowAlarms(context, this.State.Alarms);

                    this.State.AlarmIndex = 0;
                }
                else
                {
                    this.SetActiveTopic(WHICH_ALARM_PROMPT);
                    this.ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }
            }

            this.State.Alarm.Title = this.State.Alarms[(int)this.State.AlarmIndex].Title;

            if (this.State.DeleteConfirmed == null)
            {
                this.SetActiveTopic(CONFIRM_DELETE_PROMPT);
                this.ActiveTopic.OnReceiveActivity(context);
                return Task.CompletedTask;
            }

            this.OnSuccess(context, new DeleteAlarmTopicValue
            {
                Alarm = this.State.Alarm,
                AlarmIndex = (int)this.State.AlarmIndex,
                DeleteConfirmed = (bool)this.State.DeleteConfirmed
            });
            return Task.CompletedTask;
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

    public class YesOrNoValidator : Validator<bool>
    {
        public override ValidatorResult<bool> Validate(IBotContext context)
        {
            var message = context.Request.AsMessageActivity().Text.ToLowerInvariant();

            if (message == "yes")
            {
                return new ValidatorResult<bool>
                {
                    Value = true
                };
            }
            else if(message == "no")
            {
                return new ValidatorResult<bool>
                {
                    Value = false
                };
            }
            else
            {
                return new ValidatorResult<bool>
                {
                    Reason = "notyesorno"
                };
            }
        }
    }
}
