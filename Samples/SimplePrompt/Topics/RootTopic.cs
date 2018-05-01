using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using PromptlyBot;
using System.Threading.Tasks;
using PromptlyBot.Prompts;

namespace SimplePrompt.Topics
{
    public class RootTopicState : ConversationTopicState
    {
        public string Name { get; set; }
        public int? Age { get; set; }
    }

    public class RootTopic : TopicsRoot<BotConversationState, RootTopicState>
    {
        public RootTopic(ITurnContext turnContext) : base(turnContext)
        {
            this.SubTopics.Add("namePrompt", (object[] args) =>
            {
                var namePrompt = new TextPrompt();

                namePrompt.Set
                    .OnPrompt("What is your name?")
                    .OnSuccess((turn, value) =>
                    {
                        ClearActiveTopic();

                        State.Name = value;
                        // Returns a Task, should be returned so OnTurn that called it can return that.
                        OnTurn(turn);
                    });

                return namePrompt;
            });

            this.SubTopics.Add("agePrompt", (object[] args) =>
            {
                var agePrompt = new IntPrompt();

                agePrompt.Set
                    .OnPrompt("How old are you?")
                    .OnSuccess((turn, value) =>
                    {
                        ClearActiveTopic();

                        State.Age = value;

                        OnTurn(turn);
                    });

                return agePrompt;
            });
        }

        public override Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Check to see if there is an active topic.
                if (HasActiveTopic)
                {
                    // Let the active topic handle this turn by passing context to it's OnReceiveActivity().
                    return ActiveTopic
                        .OnTurn(turnContext);
                }

                // If you don't have the state you need, prompt for it
                if (State.Name == null)
                {
                    return SetActiveTopic("namePrompt")
                        .OnTurn(turnContext);
                }

                if (State.Age == null)
                {
                    return SetActiveTopic("agePrompt")
                        .OnTurn(turnContext);
                }

                return turnContext.SendActivity($"Hello { State.Name }! You are { State.Age } years old.");
            }

            return Task.CompletedTask;
        }
    }
}