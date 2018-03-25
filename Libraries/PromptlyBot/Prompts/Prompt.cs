using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PromptlyBot.Validator;
using System;
using System.Threading.Tasks;
using System.Linq;
using PromptlyBot.Validators;

namespace PromptlyBot.Prompts
{ 
    public class PromptState
    {
        public int? turns;
    }

    public class Prompt<TValue> : Topic<PromptState, TValue>
    {   
        private readonly PromptFluentInterface _set;

        public Prompt() : base ()
        {
            this._set = new PromptFluentInterface(this);
        }

        new public PromptFluentInterface Set { get => _set; }

        private Action<IBotContext, string> _onPrompt = (context, lastTurnReason) => { };
        public Action<IBotContext, string> OnPrompt { get => _onPrompt; set => _onPrompt = value; }

        private int _maxTurns = int.MaxValue;
        public int MaxTurns { get => _maxTurns; set => _maxTurns = value; }

        private Validator<TValue> _validator;
        public Validator<TValue> Validator { get => _validator; set => _validator = value; }

        public override Task OnReceiveActivity(IBotContext context)
        {
            // If this is the initial turn (turn 0), send the initial prompt.
            if (this._state.turns == null)
            {
                this._state.turns = 0;

                _onPrompt(context, null);
                return Task.CompletedTask;
            }

            // For all subsequent turns...

            // Validate the message/reply from the last turn.
            var validationResult = _validator.Validate(context);

            // If the message/reply wasn't a valid response to the prompt...
            if (validationResult.Reason != null)
            {
                // Increase the turn count.
                this._state.turns += 1;

                // If max turns has been reached, the prompt has failed with too many attempts.
                if (this._state.turns == _maxTurns)
                {
                    validationResult.Reason = "toomanyattempts";

                    OnFailure(context, validationResult.Reason);
                    return Task.CompletedTask;
                }

                // Re-prompt, providing the validation reason from last turn.
                _onPrompt(context, validationResult.Reason);
                return Task.CompletedTask;
            }

            // Prompt was successful, so pass value (result) of the Prompt.
            this.OnSuccess(context, validationResult.Value);
            return Task.CompletedTask;
        }

        public class PromptFluentInterface
        {
            private readonly Prompt<TValue> _prompt;

            public PromptFluentInterface(Prompt<TValue> prompt)
            {
                this._prompt = prompt;
            }

            public PromptFluentInterface OnPrompt(Action<IBotContext, string> onPrompt)
            {
                _prompt._onPrompt = onPrompt;
                return this;
            }

            private Action<IBotContext, string> CreateOnPrompt(params IActivity[] activities)
            {
                return (context, lastTurnReason) => {
                    context.SendActivity(activities);
                };
            }

            public PromptFluentInterface OnPrompt(params string[] textRepliesToSend)
            {
                var activities = textRepliesToSend
                    .Select(t => new Activity(ActivityTypes.Message) { Text = t })
                    .ToArray();

                this.OnPrompt(this.CreateOnPrompt(activities));
                return this;
            }

            public PromptFluentInterface OnPrompt(params IActivity[] activities)
            {
                this.OnPrompt(this.CreateOnPrompt(activities));
                return this;
            }

            public PromptFluentInterface MaxTurns(int maxTurns)
            {
                _prompt._maxTurns = maxTurns;
                return this;
            }

            public PromptFluentInterface Validator(Validator<TValue> validator)
            {
                _prompt._validator = validator;
                return this;
            }

            public PromptFluentInterface OnSuccess(Action<IBotContext, TValue> onSuccess)
            {
                _prompt.OnSuccess = onSuccess;
                return this;
            }

            public PromptFluentInterface OnFailure(Action<IBotContext, string> onFailure)
            {
                _prompt.OnFailure = onFailure;
                return this;
            }
        }

    }
}
