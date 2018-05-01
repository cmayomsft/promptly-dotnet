using AlarmBot.Models;
using AlarmBot.Views;
using Microsoft.Bot.Builder;
using PromptlyBot;
using PromptlyBot.Prompts;
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
                    .OnPrompt((turn, lastTurnReason) =>
                    {
                        if ((lastTurnReason != null) && (lastTurnReason == "indexnotfound"))
                        {
                            turn.SendActivity($"Sorry, I coulnd't find an alarm named '{ turn.Activity.Text }'.", 
                                "Let's try again.");
                        }

                        AlarmsView.ShowAlarms(turn, State.Alarms);

                        turn.SendActivity("Which alarm would you like to delete?");
                    })
                    .Validator(new AlarmIndexValidator(State.Alarms))
                    .MaxTurns(2)
                    .OnSuccess((turn, index) =>
                        {
                            ClearActiveTopic();

                            State.AlarmIndex = index;

                            OnTurn(turn);
                        })
                    .OnFailure((turn, reason) =>
                        {
                            ClearActiveTopic();

                            if ((reason != null) && (reason == "toomanyattempts"))
                            {
                                turn.SendActivity("I'm sorry I'm having issues understanding you.");
                            }

                            OnFailure(turn, reason);
                        });

                return whichAlarmPrompt;
            });

            this.SubTopics.Add(CONFIRM_DELETE_PROMPT, (object[] args) =>
            {
                var confirmDeletePrompt = new Prompt<bool>();

                confirmDeletePrompt.Set
                    .OnPrompt((turn, lastTurnReason) =>
                    {
                        if ((lastTurnReason != null) & (lastTurnReason == "notyesorno"))
                        {
                            turn.SendActivity("Sorry, I was expecting 'yes' or 'no'.",
                                "Let's try again.");
                        }

                        turn.SendActivity($"Are you sure you want to delete alarm '{ this.State.Alarm.Title }' ('yes' or 'no')?`");
                    })
                    .Validator(new YesOrNoValidator())
                    .MaxTurns(2)
                    .OnSuccess((turn, value) =>
                        {
                            ClearActiveTopic();

                            State.DeleteConfirmed = value;

                            OnTurn(turn);
                        })
                    .OnFailure((turn, reason) =>
                        {
                            ClearActiveTopic();

                            if ((reason != null) && (reason == "toomanyattempts"))
                            {
                                turn.SendActivity("I'm sorry I'm having issues understanding you.");
                            }

                            OnFailure(turn, reason);
                        });

                return confirmDeletePrompt;
            });

        }

        public override Task OnTurn(ITurnContext turnContext)
        {
            if (HasActiveTopic)
            {
                return ActiveTopic.OnTurn(turnContext);
            }

            // If there are no alarms to delete...
            if (State.Alarms.Count == 0)
            {
                return turnContext.SendActivity("There are no alarms to delete.");
            }

            if (State.AlarmIndex == null)
            {
                // If there is only one alarm to delete, use that index. No need to prompt.
                if (State.Alarms.Count == 1)
                {
                    AlarmsView.ShowAlarms(turnContext, State.Alarms);

                    State.AlarmIndex = 0;
                }
                else
                {
                    return SetActiveTopic(WHICH_ALARM_PROMPT)
                        .OnTurn(turnContext);
                }
            }

            State.Alarm.Title = State.Alarms[(int)State.AlarmIndex].Title;

            if (State.DeleteConfirmed == null)
            {
                return SetActiveTopic(CONFIRM_DELETE_PROMPT)
                    .OnTurn(turnContext);
            }

            OnSuccess(turnContext, 
                new DeleteAlarmTopicValue
                    {
                        Alarm = this.State.Alarm,
                        AlarmIndex = (int)State.AlarmIndex,
                        DeleteConfirmed = (bool)State.DeleteConfirmed
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

        public override ValidatorResult<int> Validate(ITurnContext turnContext)
        {
            int index = _alarms.FindIndex(alarm => alarm.Title.ToLowerInvariant() == turnContext.Activity.Text.ToLowerInvariant());

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
        public override ValidatorResult<bool> Validate(ITurnContext turnContext)
        {
            var message = turnContext.Activity.Text.ToLowerInvariant();

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
