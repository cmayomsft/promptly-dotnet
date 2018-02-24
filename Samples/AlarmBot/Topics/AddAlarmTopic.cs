using AlarmBot.Models;
using Microsoft.Bot.Builder;
using PromptlyBot;
using PromptlyBot.Validator;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    public class AddAlarmTopicState : ConversationTopicState
    {
        public Alarm alarm;
    }

    public class AddAlarmTopic : ConversationTopic<AddAlarmTopicState, Alarm>
    {
        public override Task OnReceiveActivity(IBotContext context)
        {
            this.OnSuccess(context, new Alarm { Title = "Chris", Time = "1 PM"});

            return Task.CompletedTask;
        }
    }

    /*public class AlarmTitleValidator : Validator<string>
    {
        public override ValidatorResult<string> Validate(IBotContext context)
        {
            if (context.Request.AsMessageActivity().Text.Length > 20)
            {
                return new ValidatorResult<string>
                {
                    Reason = "titletoolong"
                };
            }
            else
            {
                return new ValidatorResult<string>
                {
                    Value = context.Request.AsMessageActivity().Text
                };
            }
        }
    }*/

}
