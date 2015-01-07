using Crane.Core.Commands;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands
{
    public class CommandExtensionTests
    {
        [Scenario]
        public void can_get_a_commands_name_correctly(ICraneCommand command, string result)
        {
            "Given I have a command"
                ._(() => command = new Help());

            "When I get the command name"
                ._(() => result = command.Name());

            "It should be the type name in lower case"
                ._(() => result.Should().Be("help"));
        }
    }
}