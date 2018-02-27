using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmBot.Controllers
{
    public abstract class BotController : Controller
    {
        protected readonly BotFrameworkAdapter _adapter;

        public BotController(BotFrameworkAdapter adapter)
        {
            this._adapter = adapter;
        }

        protected abstract Task OnReceiveActivity(IBotContext context);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Activity activity)
        {
            try
            {
                await _adapter.ProcessActivty(this.Request.Headers["Authorization"].FirstOrDefault(), activity, OnReceiveActivity);
                return this.Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }
            catch (InvalidOperationException e)
            {
                return this.NotFound(e.Message);
            }
        }
    }
}
