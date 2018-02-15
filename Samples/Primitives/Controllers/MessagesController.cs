using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace Primitives.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : BotController
    {
        public MessagesController(Bot bot) : base(bot) { }

        protected override Task OnReceiveActivity(IBotContext context)
        {
            if (context.Request.Type == ActivityTypes.Message)
            {
                var message = context.Request.AsMessageActivity().Text;

                var turnNumber = (int)(context.State.Conversation["turnNumber"] ?? 1);

                context.Reply($"Turn: {turnNumber} - You said '{ message }'!");

                context.State.Conversation["turnNumber"] = ++turnNumber;
            }

            return Task.CompletedTask;
        }
    }
}
