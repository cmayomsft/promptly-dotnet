using AlarmBot.Models;
using Microsoft.Bot.Builder;
using PromptlyBot;
using PromptlyBot.Prompts;
using PromptlyBot.Validator;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class AddAlarmTopicState : ConversationTopicState
    {
        public Alarm Alarm = new Alarm();
    }

    public class AddAlarmTopic : ConversationTopic<AddAlarmTopicState, Alarm>
    {
        private const string TITLE_PROMPT = "titlePrompt";
        private const string TIME_PROMPT = "timePrompt";

        public AddAlarmTopic() : base()
        {
            this.SubTopics.Add(TITLE_PROMPT, (object[] args) =>
            {
                var titlePrompt = new Prompt<string>();

                titlePrompt.Set
                    .OnPrompt((turn, lastTurnReason) =>
                        {
                            if ((lastTurnReason != null) && (lastTurnReason == "titletoolong"))
                            {
                                turn.SendActivity("Sorry, alarm titles must be less that 20 characters.", 
                                    "Let's try again.");
                            }

                            turn.SendActivity("What would you like to name your alarm?");
                        })
                    .Validator(new AlarmTitleValidator())
                    .MaxTurns(2)
                    .OnSuccess((turn, value) =>
                        {
                            this.ClearActiveTopic();

                            this.State.Alarm.Title = value;

                            this.OnTurn(turn);
                        })
                    .OnFailure((turn, reason) =>
                    {
                        this.ClearActiveTopic();

                        if ((reason != null) && (reason == "toomanyattempts"))
                        {
                            turn.SendActivity("I'm sorry I'm having issues understanding you.");
                        }

                        this.OnFailure(turn, reason);
                    });

                return titlePrompt;
            });

            this.SubTopics.Add(TIME_PROMPT, (object[] args) =>
            {
                var timePrompt = new Prompt<string>();

                timePrompt.Set
                    .OnPrompt((context, lastTurnReason) =>
                    {
                        context.SendActivity("What time would you like to set your alarm for?");
                    })
                    .Validator(new AlarmTimeValidator())
                    .MaxTurns(2)
                    .OnSuccess((turn, value) =>
                    {
                        this.ClearActiveTopic();

                        this.State.Alarm.Time = value;

                        this.OnTurn(turn);
                    })
                    .OnFailure((turn, reason) =>
                    {
                        this.ClearActiveTopic();

                        if ((reason != null) && (reason == "toomanyattempts"))
                        {
                            turn.SendActivity("I'm sorry I'm having issues understanding you.");
                        }

                        this.OnFailure(turn, reason);
                    });

                return timePrompt;
            });

        }

        public override Task OnTurn(ITurnContext turnContext)
        {
            if (HasActiveTopic)
            {
                return ActiveTopic.OnTurn(turnContext);
            }

            if (State.Alarm.Title == null)
            {
                var s = SimulateAsyncMethod();

                return SetActiveTopic(TITLE_PROMPT)
                    .OnTurn(turnContext);
            }

            if (State.Alarm.Time == null)
            {
                return SetActiveTopic(TIME_PROMPT)
                    .OnTurn(turnContext);
            }

            OnSuccess(turnContext, State.Alarm);
            return Task.CompletedTask;

            async Task<string> SimulateAsyncMethod()
            {
                await Task.Delay(1000);
                return "Foo!";
            }
        }
    }

    public class AlarmTitleValidator : Validator<string>
    {
        public override ValidatorResult<string> Validate(ITurnContext turnContext)
        {
            if (turnContext.Activity.Text.Length > 20)
            {
                return new ValidatorResult<string>
                {
                    Reason = "titletoolong"
                };
            }
            else
            {
                return new ValidatorResult<string>
                {
                    Value = turnContext.Activity.Text
                };
            }
        }
    }

    public class AlarmTimeValidator : Validator<string>
    {
        public override ValidatorResult<string> Validate(ITurnContext turnContext)
        {
            return new ValidatorResult<string>
            {
                Value = turnContext.Activity.Text
            };
        }
    }
}
