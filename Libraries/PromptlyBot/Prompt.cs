using Microsoft.Bot.Builder;
using PromptlyBot.Validator;
using System;
using System.Threading.Tasks;

namespace PromptlyBot
{ 
    public class PromptState
    {
        public int? turns;
    }

    public class Prompt<TValue> : Topic<PromptState, TValue>
    {   
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
        }

        private readonly PromptFluentInterface _set;

        public Prompt() : base ()
        {
            this._set = new PromptFluentInterface(this);
        }

        public PromptFluentInterface Set { get => _set; }

        private Action<IBotContext, string> _onPrompt;
        public Action<IBotContext, string> OnPrompt { get => _onPrompt; set => _onPrompt = value; }
        public Prompt<TValue> SetOnPrompt(Action<IBotContext, string> onPrompt)
        {
            this._onPrompt = onPrompt;
            return this;
        }

        private int _maxTurns = 2;
        public int MaxTurns { get => _maxTurns; set => _maxTurns = value; }
        public Prompt<TValue> SetMaxTurns(int maxTurns)
        {
            this._maxTurns = maxTurns;
            return this;
        }

        private Validator<TValue> _validator;
        public Validator<TValue> Validator { get => _validator; set => _validator = value; }
        public Prompt<TValue> SetValidator(Validator<TValue> validator)
        {
            this._validator = validator;
            return this;
        }

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
    }
}
