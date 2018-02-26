using AlarmBot.Models;
using Microsoft.Bot.Builder;
using PromptlyBot;
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
            this.SubTopics.Add(TITLE_PROMPT, () =>
            {
                var titlePrompt = new Prompt<string>();

                titlePrompt.Set
                    .OnPrompt((context, lastTurnReason) =>
                        {
                            if ((lastTurnReason != null) && (lastTurnReason == "titletoolong"))
                            {
                                context.Reply("Sorry, alarm titles must be less that 20 characters.")
                                    .Reply("Let's try again.");
                            }

                            context.Reply("What would you like to name your alarm?");
                        })
                    .Validator(new AlarmTitleValidator())
                    .MaxTurns(2)
                    .OnSuccess((context, value) =>
                        {
                            this.ClearActiveTopic();

                            this.State.Alarm.Title = value;

                            this.OnReceiveActivity(context);
                        })
                    .OnFailure((context, reason) =>
                    {
                        this.ClearActiveTopic();

                        if ((reason != null) && (reason == "toomanyattempts"))
                        {
                            context.Reply("I'm sorry I'm having issues understanding you.");
                        }

                        this.OnFailure(context, reason);
                    });

                return titlePrompt;
            });

            this.SubTopics.Add(TIME_PROMPT, () =>
            {
                var timePrompt = new Prompt<string>();

                timePrompt.Set
                    .OnPrompt((context, lastTurnReason) =>
                    {
                        context.Reply("What time would you like to set your alarm for?");
                    })
                    .Validator(new AlarmTimeValidator())
                    .MaxTurns(2)
                    .OnSuccess((context, value) =>
                    {
                        this.ClearActiveTopic();

                        this.State.Alarm.Time = value;

                        this.OnReceiveActivity(context);
                    })
                    .OnFailure((context, reason) =>
                    {
                        this.ClearActiveTopic();

                        if ((reason != null) && (reason == "toomanyattempts"))
                        {
                            context.Reply("I'm sorry I'm having issues understanding you.");
                        }

                        this.OnFailure(context, reason);
                    });

                return timePrompt;
            });

        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            if (HasActiveTopic)
            {
                ActiveTopic.OnReceiveActivity(context);
                return Task.CompletedTask;
            }

            if (this.State.Alarm.Title == null)
            {
                this.SetActiveTopic(TITLE_PROMPT);
                this.ActiveTopic.OnReceiveActivity(context);
                return Task.CompletedTask;
            }

            if (this.State.Alarm.Time == null)
            {
                this.SetActiveTopic(TIME_PROMPT);
                this.ActiveTopic.OnReceiveActivity(context);
                return Task.CompletedTask;
            }

            this.OnSuccess(context, this.State.Alarm);

            return Task.CompletedTask;
        }
    }

    public class AlarmTitleValidator : Validator<string>
    {
        public override ValidatorResult<string> Validate(IBotContext context)
        {
            if (context.Request.AsMessageActivity().Text.Length > 20)
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
                    Value = context.Request.AsMessageActivity().Text
                };
            }
        }
    }

    public class AlarmTimeValidator : Validator<string>
    {
        public override ValidatorResult<string> Validate(IBotContext context)
        {
            return new ValidatorResult<string>
            {
                Value = context.Request.AsMessageActivity().Text
            };
        }
    }
}
