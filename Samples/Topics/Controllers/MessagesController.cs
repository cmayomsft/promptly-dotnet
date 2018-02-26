using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;
using Topics.Topics;

namespace Topics.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : BotController
    {
        public MessagesController(BotFrameworkAdapter adapter) : base(adapter) { }

        protected override Task OnReceiveActivity(IBotContext context)
        {
            if (context.Request.Type == ActivityTypes.Message)
            {
                var rootTopic = new RootTopic(context);

                rootTopic.OnReceiveActivity(context);
            }

            return Task.CompletedTask;
        }
    }
}
