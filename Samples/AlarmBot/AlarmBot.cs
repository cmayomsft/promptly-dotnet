using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;

namespace AlarmBot
{
    public class AlarmBotState : StoreItem
    {
        public int TurnNumber { get; set; }
    }

    public class AlarmBot : IBot
    {
        public Task OnReceiveActivity(IBotContext botContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
