using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;

namespace PromptlyBot
{
    public class PromptlyBotConversationState<TRootTopicState> 
        where TRootTopicState : ConversationTopicState, new()
    {
        public TRootTopicState RootTopic { get; set; }
    }
    
    public abstract class TopicsRoot<TConversationState, TState> : ConversationTopic<TState> 
        where TConversationState : PromptlyBotConversationState<TState>, new()
        where TState : ConversationTopicState, new()
    {
        public TopicsRoot(ITurnContext turnContext) : base()
        {
            if (turnContext.GetConversationState<TConversationState>().RootTopic == null)
            {
                turnContext.GetConversationState<TConversationState>().RootTopic = new TState();
            }

            this.State = turnContext.GetConversationState<TConversationState>().RootTopic;
        }
    }
}
