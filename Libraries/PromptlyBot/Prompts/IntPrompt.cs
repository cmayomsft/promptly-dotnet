using Microsoft.Bot.Schema;
using PromptlyBot.Validators;
using System;
using System.Collections.Generic;
using System.Text;

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
