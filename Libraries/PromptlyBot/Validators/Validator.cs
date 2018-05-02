using Microsoft.Bot.Builder;

namespace PromptlyBot.Validator
{
    public class ValidatorResult<TValue>
    {
        public ValidatorResult() { }
        public TValue Value { get; set; }
        public string Reason { get; set; }
    }

    public abstract class Validator<TValue>
    {
        public abstract ValidatorResult<TValue> Validate(ITurnContext context);
    }
}
