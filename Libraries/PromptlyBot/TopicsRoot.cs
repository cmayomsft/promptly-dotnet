using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromptlyBot
{
    public abstract class TopicsRoot : ConversationTopic<ConversationTopicState>
    {
        public TopicsRoot(IBotContext context) : base(null)
        {
            if (context.State.Conversation["RootTopic"] == null)
            {
                context.State.Conversation["RootTopic"] = new ConversationTopicState();
            }

            this.State = context.State.Conversation["RootTopic"];
        }
    }
}
