using System.Collections.Generic;
using Crane.Core.Commands;
using Crane.Core.Documentation;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Documentation
{
    public class CommandHelpTests
    {
        [Scenario]
        public void object_instantiation(ICommandHelp commandHelp)
        {
            "When I create an instance of command help"
                ._(() => commandHelp = new CommandHelp("init", "Crane.Core.Commands.Init", string.Empty, new List<CommandExample>()));

            "Then it should create the command type based on the supplied full name"
                ._(() => commandHelp.CommandType.FullName.Should().Be(typeof (Init).FullName));
        }
    }
}