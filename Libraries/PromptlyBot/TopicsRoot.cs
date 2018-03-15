using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;

namespace PromptlyBot
{
    public class TopicsRootState : StoreItem
    {
        public ConversationTopicState RootTopic { get; set; }
    }
    
    public abstract class TopicsRoot<TConversationState> : ConversationTopic<ConversationTopicState> where TConversationState : TopicsRootState, new()
    {
        public TopicsRoot(IBotContext context) : base()
        {
            if (context.GetConversationState<TConversationState>().RootTopic == null)
            {
                context.GetConversationState<TConversationState>().RootTopic = new ConversationTopicState();
            }

            this.State = context.GetConversationState<TConversationState>().RootTopic;
        }
    }
}
