using PromptlyBot.Validators;

namespace PromptlyBot.Prompts
{
    public class TextPrompt : Prompt<string>
    {
        public TextPrompt() : base()
        {
            this.Validator = new TextValidator();
        }
    }
}
