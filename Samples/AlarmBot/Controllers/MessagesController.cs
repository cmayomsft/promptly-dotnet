using AlarmBot.Topics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Threading.Tasks;

namespace AlarmBot.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : BotController
    {
        public MessagesController(Bot bot) : base(bot) { }

        // TODO: Why is this IBotContext vs. BotContext?
        // TODO: Why is this called OnReceiveActivity? Doesn't receive an Activity as an arg, different from Node OnReceive().
        protected override Task OnReceiveActivity(IBotContext context)
        {
            var rootTopic = new RootTopic(context);

            rootTopic.OnReceiveActivity(context);

            return Task.CompletedTask;
        }
    }
}
