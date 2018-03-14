using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;

namespace PromptlyBot
{
    public abstract class TopicsRoot : ConversationTopic<ConversationTopicState>
    {
        public TopicsRoot(IBotContext context) : base()
        {
            this.State = context.GetConversationState<ConversationTopicState>();
        }
    }
}
