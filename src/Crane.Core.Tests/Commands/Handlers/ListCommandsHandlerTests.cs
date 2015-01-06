using Crane.Core.Commands;
using Crane.Core.Commands.Handlers;
using Crane.Core.IO;
using Crane.Core.Tests.TestUtilities;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Handlers
{
    public class ListCommandsHandlerTests
    {
        [Scenario]
        public void should_not_list_commands_that_have_hide_attribute(ListCommandsHandler handler, IOutput output)
        {            
            "Given I have a list command handler"
                ._(() =>
                {
                    output = new MockOutput();
                    handler = new ListCommandsHandler(new ICraneCommand[]{new VisableCommand(), new HiddenCommand()}, output);                    
                });

            "When I invoke the command"
                ._(() => handler.Handle(new ListCommands()));

            "Then it should not list the hidden command"
                ._(() => output.ToString().Should().NotContain("crane hiddencommand"));
        }

        public class VisableCommand : ICraneCommand { }

        [Crane.Core.Commands.Attributes.HiddenCommand]
        public class HiddenCommand : ICraneCommand { }
    }
}