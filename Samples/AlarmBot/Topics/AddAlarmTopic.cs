using AlarmBot.Models;
using Microsoft.Bot.Builder;
using PromptlyBot;
using PromptlyBot.Validator;
using System.Threading.Tasks;

namespace AlarmBot.Topics
{
    /*public class AddAlarmTopic : ConversationTopic<Alarm>
    {
        private Alarm _alarm;

        public override Task OnReceiveActivity(IBotContext context)
        {
            if(HasActiveTopic)
            {
                this.ActiveTopic.OnReceiveActivity(context);
                return Task.CompletedTask;
            }

            if (_alarm.Title == null)
            {


            }

            return Task.CompletedTask;
        }
    }*/

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
