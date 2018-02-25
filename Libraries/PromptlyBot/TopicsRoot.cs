using Microsoft.Bot.Builder;

namespace PromptlyBot
{
    public abstract class TopicsRoot : ConversationTopic<ConversationTopicState>
    {
        public TopicsRoot(IBotContext context) : base()
        {
            if (context.State.Conversation["RootTopic"] == null)
            {
                context.State.Conversation["RootTopic"] = new ConversationTopicState();
            }

            this.State = context.State.Conversation["RootTopic"];
        }
    }
}
