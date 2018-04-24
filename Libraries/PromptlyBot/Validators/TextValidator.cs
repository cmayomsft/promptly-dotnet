using Microsoft.Bot.Builder;
using PromptlyBot.Validator;
using System;

namespace PromptlyBot.Validators
{
    public class TextValidator : Validator<string>
    {
        public override ValidatorResult<string> Validate(IBotContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.AsMessageActivity().Text))
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
}
