using System.Collections.Generic;
using Crane.Core.Commands;
using Crane.Core.Documentation;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Documentation
{
    public class CommandHelpCollectionTests
    {
        [Scenario]
        public void can_retrieve_command_help_specifying_a_commands_short_name(CommandHelpCollection help, ICraneCommand command, ICommandHelp result)
        {
            "Given I have a help collection"
                ._(() => help = new CommandHelpCollection(new Dictionary<string, ICommandHelp>
                {
                    {"init", new CommandHelp("init", "Crane.Core.Commands.Init", "Initializes things", null)},
                    {"help", new CommandHelp("help", "Crane.Core.Commands.Help", "Helps things", null)},
                }));

            "When I retrieve the command help using a short name"
                ._(() => result = help.Get("help"));

            "Then it should retrieve the command help associated with that type"
                ._(() => result.CommandName.Should().Be("help"));
        }
    }
}