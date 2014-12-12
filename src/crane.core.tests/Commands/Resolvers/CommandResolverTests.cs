using System;
using System.Collections.Generic;
using Crane.Core.Commands;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Commands.Resolvers;
using Crane.Integration.Tests.TestUtilities;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Resolvers
{
    public class CommandResolverTests
    {
        [Scenario]
        public void Throws_unknown_command_exception_if_none_match(CommandResolver commandResolver, List<ICraneCommand> commands, Exception result)
        {
            "Given I have the commands init and help"
                ._(() =>
                {
                    commandResolver = new CommandResolver();
                    commands = new List<ICraneCommand> { B.AutoMock<Help>().Subject, B.AutoMock<Init>().Subject };
                } );

            "When I resolve the command bob"
                ._(() => result = Throws.Exception(() => commandResolver.Resolve(commands, "bob")));

            "Then an UnknownCommandCraneException is thrown"
                ._(() => result.Should().BeOfType<UnknownCommandCraneException>());

            "And the exception message should detail the command passed in"
                ._(() => result.Message.Should().Contain("bob"));


        }

        [Scenario]
        public void Resolve_gives_command_if_match(CommandResolver commandResolver, List<ICraneCommand> commands, Type result)
        {
            "Given I have the commands init and help"
               ._(() =>
               {
                   commandResolver = new CommandResolver();
                   commands = new List<ICraneCommand> { B.AutoMock<Help>().Subject, B.AutoMock<Init>().Subject };
               });

            "When I resolve the command Init"
                ._(() => result = commandResolver.Resolve(commands, "Init"));

            "Then the init command is returned"
                ._(() => result.Should().Be(typeof(Init)));
        }

        [Scenario]
        public void Resolve_does_not_care_about_case(CommandResolver commandResolver, List<ICraneCommand> commands, Type result)
        {
            "Given I have the commands init and help"
               ._(() =>
               {
                   commandResolver = new CommandResolver();
                   commands = new List<ICraneCommand> { B.AutoMock<Help>().Subject, B.AutoMock<Init>().Subject };
               });

            "When I resolve the command init"
                ._(() => result = commandResolver.Resolve(commands, "init"));

            "Then the init command is returned"
                ._(() => result.Should().Be(typeof(Init)));
        }
    }
}
