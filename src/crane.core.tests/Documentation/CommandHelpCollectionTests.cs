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
        public void can_retrieve_command_help_specifying_a_type(CommandHelpCollection help, ICraneCommand command, ICommandHelp result)
        {
            "Given I have a help collection"
                ._(() => help = new CommandHelpCollection(new Dictionary<string, ICommandHelp>
                {
                    {"Crane.Core.Commands.Init", new CommandHelp("init", "Initializes things", null)},
                    {"Crane.Core.Commands.Help", new CommandHelp("help", "Helps things", null)},
                }));

            "When I retrieve the command help using a type"
                ._(() => result = help.Get<Init>());

            "It should retrieve the command help associated with that type"
                ._(() => result.CommandName.Should().Be("init"));
        }
    }
}