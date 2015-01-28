using Crane.Core.Configuration;
using Crane.Core.Documentation;
using Crane.Core.Documentation.Formatters;
using Crane.Core.Extensions;
using Crane.Core.Tests.TestUtilities;
using FakeItEasy;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Documentation.Formatters
{
    public class ConsoleHelpFormatterTests
    {
        [Scenario]
        public void formatting_help_command_with_console_formatter(IHelpFormatter formatter, ICommandHelp commandHelp, string result)
        {
            "Given I have a console formatter"
                ._(() => formatter = ServiceLocator.Resolve<ConsoleHelpFormatter>());

            "And I have a help command"
                ._(() => commandHelp = new CommandHelp("init", "Crane.Core.Commands.Init", "Initializes a new project with foo and bar.", new[] { new CommandExample{ Value = "Example 1"} }));

            "When I format the command"
                ._(() => result = formatter.Format(commandHelp));

            "It should display the usage statement on the first line"
                ._(() => result.Line(0).Should().Be("usage: crane init <project name>"));

            "It should display the description statement on the 3rd line"
                ._(() => result.Line(2).Should().Be("Initializes a new project with foo and bar."));

            "It should display the example"
                ._(() => result.Line(4).Should().Be("Example 1"));

            "It should display for information url"
                ._(() => result.Line(6).Should().Contain("For more information, visit"));
        }
    }
}