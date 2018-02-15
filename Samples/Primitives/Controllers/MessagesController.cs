using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;

namespace Primitives.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly Bot _bot;

        public MessagesController(Bot bot)
        {
            _bot = bot;

            _bot.OnReceive((context, nextDelegate) =>
            {
                if (context.Request.Type == ActivityTypes.Message)
                {
                    var userMessage = context.Request.AsMessageActivity().Text;

                    var turnNumber = (int)(context.State.Conversation[@"turnNumber"] ?? 1);
                    context.Reply($@"Turn: {turnNumber++} - You said '{userMessage}'!");
                }

                return Task.CompletedTask;
            });
        }

        [HttpPost]
        public void HandleActivityFromUser([FromBody] Activity activity)
        {
            ((BotFrameworkAdapter)_bot.Adapter).Receive(null, activity);
        }

    }
}
