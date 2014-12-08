using Crane.Core.Commands;
using Crane.Core.Commands.Handlers;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Commands
{
    public class CommandFactoryFeature
    {
        [Scenario]
        public void Can_fill_arguments_to_init_command_when_passed_by_position(ICommandFactory commandFactory, ICraneCommand craneCommand)
        {
            "Given I have a command factory"
                ._(() => commandFactory = ioc.Resolve<ICommandFactory>());

            "When I create a command with based on the arguments 'init testproject"
                ._(() => craneCommand = commandFactory.Create(new[] { "init", "testproject" }));

            "Then the command returned should be the init command"
                ._(() => craneCommand.Should().BeOfType<InitCommand>());

            "And the ProjectName should be testproject"
                ._(() => ((InitCommand) craneCommand).ProjectName.Should().Be("testproject"));
        }
    }
}
