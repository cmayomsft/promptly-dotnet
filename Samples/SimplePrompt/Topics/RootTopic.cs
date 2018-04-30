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
                        this.ClearActiveTopic();

                        this.State.Name = value;

                        this.OnTurn(turn);
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
                        this.ClearActiveTopic();

                        this.State.Age = value;

                        this.OnTurn(turn);
                    });

                return agePrompt;
            });
        }

        public override Task OnTurn(ITurnContext turnContext)
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Check to see if there is an active topic.
                if (this.HasActiveTopic)
                {
                    // Let the active topic handle this turn by passing context to it's OnReceiveActivity().
                    this.ActiveTopic.OnTurn(turnContext);
                    return Task.CompletedTask;
                }

                // If you don't have the state you need, prompt for it
                if (this.State.Name == null)
                {
                    this.SetActiveTopic("namePrompt")
                        .OnTurn(turnContext);
                    return Task.CompletedTask;
                }

                if (this.State.Age == null)
                {
                    this.SetActiveTopic("agePrompt")
                        .OnTurn(turnContext);
                    return Task.CompletedTask;
                }

                // Now that you have the state you need (age and name), use it!
                turnContext.SendActivity($"Hello { this.State.Name }! You are { this.State.Age } years old.");
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}