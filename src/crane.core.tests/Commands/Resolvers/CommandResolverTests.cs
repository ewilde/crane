using System.Collections.Generic;
using Crane.Core.Commands;
using Crane.Core.Commands.Resolvers;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Resolvers
{
    public class CommandResolverTests
    {
        [Scenario]
        public void Resolve_gives_help_command_if_none_match(CommandResolver commandResolver, List<ICraneCommand> commands, ICraneCommand result)
        {
            "Given I have the commands init and help"
                ._(() =>
                {
                    commandResolver = new CommandResolver();
                    commands = new List<ICraneCommand> { B.AutoMock<Help>().Subject, B.AutoMock<Init>().Subject };
                } );

            "When I resolve the command bob"
                ._(() => result = commandResolver.Resolve(commands, "bob"));

            "Then the help command is returned"
                ._(() => result.Should().BeOfType<Help>());


        }

        [Scenario]
        public void Resolve_gives_command_if_match(CommandResolver commandResolver, List<ICraneCommand> commands, ICraneCommand result)
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
                ._(() => result.Should().BeOfType<Init>());
        }

        [Scenario]
        public void Resolve_does_not_care_about_case(CommandResolver commandResolver, List<ICraneCommand> commands, ICraneCommand result)
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
                ._(() => result.Should().BeOfType<Init>());
        }
    }
}
