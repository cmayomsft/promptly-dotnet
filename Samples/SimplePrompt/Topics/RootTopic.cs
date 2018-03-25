using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Schema;
using PromptlyBot;
using PromptlyBot.Validator;
using System;
using System.Threading.Tasks;

namespace SimplePrompt.Topics
{
    public class RootTopicState : ConversationTopicState
    {
        public string Name { get; set; }
        public int? Age { get; set; }
    }

    public class RootTopic : TopicsRoot<BotConversationState, RootTopicState>
    {
        public RootTopic(IBotContext context) : base(context)
        {
            this.SubTopics.Add("namePrompt", (object[] args) =>
            {
                var namePrompt = new Prompt<string>();

                namePrompt.Set
                    .OnPrompt((ctx, lastTurnReason) =>
                    {
                        context.SendActivity("What is your name?");
                    })
                    .Validator(new TextValidator())
                    .MaxTurns(2)
                    .OnSuccess((ctx, value) =>
                    {
                        this.ClearActiveTopic();

                        this.State.Name = value;

                        this.OnReceiveActivity(context);
                    })
                    .OnFailure((ctx, reason) =>
                    {
                        this.ClearActiveTopic();

                        context.SendActivity("I'm sorry I'm having issues understanding you.");
                    });

                return namePrompt;
            });

            this.SubTopics.Add("agePrompt", (object[] args) =>
            {
                var agePrompt = new Prompt<int>();

                agePrompt.Set
                    .OnPrompt((ctx, lastTurnReason) =>
                    {
                        context.SendActivity("How old are you?");
                    })
                    .Validator(new IntValidator())
                    .MaxTurns(2)
                    .OnSuccess((ctx, value) =>
                    {
                        this.ClearActiveTopic();

                        this.State.Age = value;

                        this.OnReceiveActivity(context);
                    })
                    .OnFailure((ctx, reason) =>
                    {
                        this.ClearActiveTopic();

                        context.SendActivity("I'm sorry I'm having issues understanding you.");
                    });

                return agePrompt;
            });
        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            if (context.Request.Type == ActivityTypes.Message)
            {
                // Check to see if there is an active topic.
                if (this.HasActiveTopic)
                {
                    // Let the active topic handle this turn by passing context to it's OnReceiveActivity().
                    this.ActiveTopic.OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                // Prompt for name if we don't have it
                if (this.State.Name == null)
                {
                    this.SetActiveTopic("namePrompt")
                        .OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                // Prompt for age if we don't have it
                if (this.State.Age == null)
                {
                    this.SetActiveTopic("agePrompt")
                        .OnReceiveActivity(context);
                    return Task.CompletedTask;
                }

                // Now that we have the state we need (age and name), use it!
                context.SendActivity($"Hello { this.State.Name }! You are { this.State.Age } years old.");
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }

    public class TextValidator : Validator<string>
    {
        public override ValidatorResult<string> Validate(IBotContext context)
        {
            if ((context.Request.AsMessageActivity().Text != null) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                return new ValidatorResult<string>
                {
                    Value = context.Request.AsMessageActivity().Text
                };
            }
            else
            {
                return new ValidatorResult<string>
                {
                    Reason = "nottext"
                };
            }
        }
    }

    public class IntValidator : Validator<int>
    {
        public override ValidatorResult<int> Validate(IBotContext context)
        {
            int value;

            if (Int32.TryParse(context.Request.AsMessageActivity().Text, out value))
            {
                return new ValidatorResult<int>
                {
                    Value = value
                };
            }
            else
            {
                return new ValidatorResult<int>
                {
                    Reason = "notint"
                };
            }
        }
    }
}
