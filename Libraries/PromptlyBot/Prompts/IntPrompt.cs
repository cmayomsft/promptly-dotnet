using PromptlyBot.Validators;

namespace PromptlyBot.Prompts
{
    public class IntPrompt : Prompt<int>
    {
        public IntPrompt() : base()
        {
            this.Validator = new IntValidator();
        }
    }
}
