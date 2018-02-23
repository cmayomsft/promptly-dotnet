using Microsoft.Bot.Builder;
using PromptlyBot.Validator;
using System;
using System.Threading.Tasks;

namespace PromptlyBot
{
    public class Prompt<TValue> : Topic<TValue>
    {
        private int _turns = 0;
        public int Turns
        {
            get
            {
                return _turns;
            }
        }

        private Action<IBotContext, string> _onPrompt;
        public Prompt<TValue> OnPrompt(Action<IBotContext, string> onPrompt)
        {
            _onPrompt = onPrompt;
            return this;
        }

        private int _maxTurns = 2;
        public Prompt<TValue> MaxTurns(int maxTurns)
        {
            _maxTurns = maxTurns;
            return this;
        }

        private Validator<TValue> _validator;
        public Prompt<TValue> Validator(Validator<TValue> validator)
        {
            _validator = validator;
            return this;
        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            // If this is the initial turn (turn 0), send the initial prompt.
            if (_turns == 0)
            {
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
                _turns += 1;

                // If max turns has been reached, the prompt has failed with too many attempts.
                if (_turns == _maxTurns)
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
