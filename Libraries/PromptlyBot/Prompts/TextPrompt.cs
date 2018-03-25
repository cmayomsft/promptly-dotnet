using Microsoft.Bot.Schema;
using PromptlyBot.Validators;

namespace PromptlyBot.Prompts
{
    public class TextPrompt : Prompt<string>
    {
        public TextPrompt() : base()
        {
            this.Validator = new TextValidator();
        }

        public TextPrompt(params string[] textRepliesToSend) : this()
        {
            this.CreateOnPrompt(textRepliesToSend);
        }

        public TextPrompt(params IActivity[] activities) : this()
        {
            this.CreateOnPrompt(activities);
        }
    }
}
