using AlarmBot.Promptly.Validator;
using Microsoft.Bot.Builder;
using Promptly;
using System;
using System.Threading.Tasks;

namespace AlarmBot.Promptly
{
    public class Prompt<TValue> : Topic<TValue>
    {
        public int Turns { get; set; } = 0;

        private Func<IBotContext, string, Task> _OnPrompt;
        public Topic OnPrompt(Func<IBotContext, string, Task> onPrompt)
        {
            _OnPrompt = onPrompt;
            return this;
        }

        private int _MaxTurns = 2;
        public Topic MaxTurns(int maxTurns)
        {
            _MaxTurns = maxTurns;
            return this;
        }

        private Validator<TValue> _Validator;
        public Topic Validator(Validator<TValue> validator)
        {
            _Validator = validator;
            return this;
        }

        public override Task OnReceiveActivity(IBotContext context)
        {
            if (this.Turns == 0)
            {
                this.OnPrompt(context);
                return
            }
        }
    }
}
