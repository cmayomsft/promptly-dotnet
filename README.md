# promptly-dotnet

Promptly is a .NET library for building chat bots using the [Bot Builder SDK V4 .NET](https://github.com/Microsoft/botbuilder-dotnet). 

Promptly provides an API for developing everything from complex, multi-turn topics of conversation to simple single-turn prompts in a simple and consistent way, so you can focus on your bot's conversational UI/UX (CUI) rather than the underlying details of conversation state and turn management.

### Prerequisites
* 1
* 2
* 3
* 4

### Packages
You can install the [Promptly-Bot package](https://www.nuget.org/packages/Promptly-Bot/) on nuget.

### Getting Started
1. X
2. Y
3. Z

To get started clone/fork this repo, open the promptly-dotnet.sln, build the solution, and run/step through the [AlarmBot sample](Samples/AlarmBot/) starting at the [`MessagesController`](Samples/AlarmBot/Controllers/MessagesController.cs).

## Overview
TBD

## Prompt Pattern in Bot Builder SDK V4
* V3 shipped with Dialogs
    * Great way to get up and running quickly with reasonable code
    * But hard to use if dialogs didn't match your scenario, required workarounds
    * Hard to understand what was going on under the covers when you had issues.
* V4 ships with primatives, context and state, that allow you to build conversational models/prompting using a commmon pattern, the Prompt Pattern. The goal SDK is to provide Primatives that will allow you/the community to build conversational abstractions to your own unique scenario. In the end, these primatives are
    * transparent
    * powerful
    * give the community something that they can use to build abstractions
    * But, in the case of the Prompt Pattern, can rquire a lot of code/convention/and in more complex conversational models, complexity to build a natural bot CUI.

Promptly is one of those abstractions.

### Prompt Pattern Example

Let's look at an example. Say, I wanted to build a simple bot that would interview the user for their name and age.

* Name/Age Primitives sample.

### Key Prompt Pattern Concepts
* Stateless Web API
* Conversations are stateful
* Turns

Promptly manages the state of the conversation between turns so you can focus on your CUI/UX of your bot.







