using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace AlarmBot.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : BotController
    {
        public MessagesController(Bot bot) : base(bot) { }

        // TODO: Why is this IBotContext?
        protected override Task OnReceiveActivity(IBotContext context)
        {
            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var message = context.Request.AsMessageActivity();

                if (message.Text.ToLowerInvariant() == "add alarm") {

                    context.Reply("Adding an alarm...");
                }

                showDefaultMessage(context);
            }

            return Task.CompletedTask;
        }

        private void showDefaultMessage(IBotContext context)
        {
            context.Reply("'Show Alarms', 'Add Alarm', 'Delete Alarm', 'Help'.");
        }
    }
}



/*
     * // If bot doesn't have state it needs, prompt for it.
    if (context.State.User["name"] == null)
    {
        // On the first turn, prompt and update state that conversation is in a prompt.
        if (context.State.Conversation["prompt"] != "name")
        {
            context.State.Conversation["prompt"] = "name";
            context.Reply("What is your name?");
            // On the subsequent turn, update state with reply and update state that prompt has completed. 
        }
        else
        {
            context.State.Conversation["prompt"] = "";
            context.State.User["name"] = context.Request.AsMessageActivity().Text;
            context.Reply($"Great, I'll call you '{ context.State.User["name"] }'!");
        }
    }
    else
    {
        context.Reply($"{ context.State.User["name"]} said: '{ context.Request.AsMessageActivity().Text }'");
    }
*/
