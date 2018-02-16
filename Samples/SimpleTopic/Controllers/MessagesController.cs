using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Threading.Tasks;
using SimpleTopics.Topics;

namespace SimpleTopics.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : BotController
    {
        public MessagesController(Bot bot) : base(bot) { }

        protected override Task OnReceiveActivity(IBotContext context)
        {

            if ((context.Request.Type == ActivityTypes.Message) && (context.Request.AsMessageActivity().Text.Length > 0))
            {
                var activeTopic = (SimpleTopic)context.State.Conversation["ActiveTopic"];

                if (activeTopic == null)
                {
                    activeTopic = new SimpleTopic();
                    context.State.Conversation["ActiveTopic"] = activeTopic;
                }

                activeTopic.OnReceiveActivity(context);
            }

            return Task.CompletedTask;
        }
    }
}
