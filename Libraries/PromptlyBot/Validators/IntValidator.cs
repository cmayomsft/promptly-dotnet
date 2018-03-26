using Microsoft.Bot.Builder;
using PromptlyBot.Validator;
using System;

namespace PromptlyBot.Validators
{

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
