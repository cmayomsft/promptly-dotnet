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
    
    // TopicsRoot needs to have the Type of ConversationState passed to it.
    // ConversationState needs to have a property RootTopic of type ConversationState.

    // TopicsRoot state must inherit from ConversationTopicState.
    // context.GetConversationState<ConversationState> must be built using that state and have a property called RootTopic of type TopicsRootState
}
