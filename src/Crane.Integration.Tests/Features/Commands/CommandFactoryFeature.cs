using Crane.Core.Commands;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Commands.Factories;
using Crane.Core.Configuration;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Commands
{
    public class CommandFactoryFeature
    {
        [Scenario]
        public void Creates_command_when_arguments_passed_by_position(ICommandFactory commandFactory, 
                                                                                ICraneCommand craneCommand)
        {
            "Given I have a command factory"
                ._(() => commandFactory = ioc.Resolve<ICommandFactory>());

            "When I create a command with based on the arguments 'init testproject"
                ._(() => craneCommand = commandFactory.Create("init", "testproject"));

            "Then the command returned should be the init command"
                ._(() => craneCommand.Should().BeOfType<Init>());

            "And the ProjectName should be testproject"
                ._(() => ((Init) craneCommand).ProjectName.Should().Be("testproject"));
        }

        [Scenario]
        public void Creates_command_when_arguments_passed_by_switches(ICommandFactory commandFactory,
                                                                                ICraneCommand craneCommand)
        {
            "Given I have a command factory"
                ._(() => commandFactory = ioc.Resolve<ICommandFactory>());

            "When I create a command with based on the arguments 'init testproject"
                ._(() => craneCommand = commandFactory.Create("init", "-projectName", "testproject"));

            "Then the command returned should be the init command"
                ._(() => craneCommand.Should().BeOfType<Init>());

            "And the ProjectName should be testproject"
                ._(() => ((Init)craneCommand).ProjectName.Should().Be("testproject"));
        }

        [Scenario]
        public void Throws_exception_detailing_missing_argument_when_all_arguments_not_passed(
            ICommandFactory commandFactory,
            CraneException craneException)
        {
            "Given I have a command factory"
                ._(() => commandFactory = ioc.Resolve<ICommandFactory>());

            "When I create a command with based on the arguments 'init'"
                ._(() => craneException = Throws.Exception(() => commandFactory.Create("init")) as CraneException);

            "Then a missing argument exception should be thrown"
                ._(() => craneException.Should().BeOfType<MissingArgumentCraneException>());

            "And the message should contain -projectName"
                ._(() => craneException.Message.Should().Contain("ProjectName"));

        }

        [Scenario]
        public void Creates_listcommands_command_when_no_arguments_passed(ICommandFactory commandFactory,
                                                                                ICraneCommand craneCommand)
        {
            "Given I have a command factory"
                ._(() => commandFactory = ioc.Resolve<ICommandFactory>());

            "When I create a command with no arguments"
                ._(() => craneCommand = commandFactory.Create());

            "Then the command returned should be the list commands command"
                ._(() => craneCommand.Should().BeOfType<ListCommands>());

        }

        [Scenario]
        public void Creates_listcommands_command_when_null_arguments_passed(ICommandFactory commandFactory,
                                                                                ICraneCommand craneCommand)
        {
            "Given I have a command factory"
                ._(() => commandFactory = ioc.Resolve<ICommandFactory>());

            "When I create a command with no arguments"
                ._(() => craneCommand = commandFactory.Create(null));

            "Then the command returned should be the list commands command"
                ._(() => craneCommand.Should().BeOfType<ListCommands>());

        }
    }
}
