using Microsoft.Bot.Builder;
using PromptlyBot.Validator;
using System;
using System.Threading.Tasks;

namespace PromptlyBot
{
    public class PromptState
    {
        public int turns;
    }

    public class Prompt<TValue> : Topic<PromptState, TValue>
    {
        private Action<IBotContext, string> _onPrompt;
        public Action<IBotContext, string> OnPrompt { get => _onPrompt; set => _onPrompt = value; }

        private int _maxTurns = 2;
        public int MaxTurns { get => _maxTurns; set => _maxTurns = value; }

        private Validator<TValue> _validator;
        public Validator<TValue> Validator { get => _validator; set => _validator = value; }

        public override Task OnReceiveActivity(IBotContext context)
        {
            // If this is the initial turn (turn 0), send the initial prompt.
            if (this._state.turns == 0)
            {
                this._state.turns += 1;

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
