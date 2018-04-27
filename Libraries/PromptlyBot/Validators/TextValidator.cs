using Microsoft.Bot.Builder;
using PromptlyBot.Validator;
using System;

namespace PromptlyBot.Validators
{
    public class TextValidator : Validator<string>
    {
        public override ValidatorResult<string> Validate(ITurnContext context)
        {
            if ((context.Activity.Text != null) && (context.Activity.Text.Length > 0))
            {
                return new ValidatorResult<string>
                {
                    Value = context.Activity.Text
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
