using Microsoft.Bot.Builder;

namespace PromptlyBot
{
    public abstract class TopicsRoot : ConversationTopic<ConversationTopicState>
    {
        public TopicsRoot(IBotContext context) : base()
        {
            if (context.State.ConversationProperties["RootTopic"] == null)
            {
                context.State.ConversationProperties["RootTopic"] = new ConversationTopicState();
            }

            this.State = context.State.ConversationProperties["RootTopic"];
        }
    }
}
