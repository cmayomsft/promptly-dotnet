using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;

namespace PromptlyBot
{
    public class PromptlyBotConversationState<TRootTopicState> : StoreItem 
        where TRootTopicState : ConversationTopicState, new()
    {
        public TRootTopicState RootTopic { get; set; }
    }
    
    public abstract class TopicsRoot<TConversationState, TState> : ConversationTopic<TState> 
        where TConversationState : PromptlyBotConversationState<TState>, new()
        where TState : ConversationTopicState, new()
    {
        public TopicsRoot(IBotContext context) : base()
        {
            if (context.GetConversationState<TConversationState>().RootTopic == null)
            {
                context.GetConversationState<TConversationState>().RootTopic = new TState();
            }

            this.State = context.GetConversationState<TConversationState>().RootTopic;
        }
    }
}
