using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Middleware;
using Microsoft.Bot.Schema;

namespace SimpleTopics.Controllers
{
    public abstract class BotController : Controller
    {
        protected readonly BotFrameworkAdapter _adapter;

        public BotController(Bot bot)
        {
            _adapter = (BotFrameworkAdapter)bot.Adapter;
            bot.OnReceive(BotReceiveHandler);
        }

        private Task BotReceiveHandler(IBotContext context, MiddlewareSet.NextDelegate next) => OnReceiveActivity(context);

        protected abstract Task OnReceiveActivity(IBotContext context);

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody]Activity activity)
        {
            try
            {
                await _adapter.Receive(this.Request.Headers["Authorization"].FirstOrDefault(), activity);
                return this.Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }
        }
    }
}