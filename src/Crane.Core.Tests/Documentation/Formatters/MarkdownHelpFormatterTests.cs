using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Documentation;
using Crane.Core.Documentation.Formatters;
using Crane.Core.Tests.TestUtilities;
using Crane.Core.Utility;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Documentation.Formatters
{
    public class MarkdownHelpFormatterTests
    {
        [Scenario]
        public void formatting_help_command_with_console_formatter(IHelpFormatter formatter, ICommandHelp commandHelp, string result)
        {
            "Given I have a markdown formatter"
                ._(() => formatter = ioc.Resolve<MarkdownHelpFormatter>());

            "And I have a help command"
                ._(() => commandHelp = new CommandHelp("init", "Crane.Core.Commands.Init", 
                    "Initializes a new project with foo and bar.", 
                    new[] { new CommandExample{ Value = "Example 1\n<code>\nvar x = 1;\nvar y=2;\n</code>"} }));

            "When I format the command"
                ._(() => result = formatter.Format(commandHelp));

            "It should display the usage statement on the first line"
                ._(() => result.Line(0).Should().Be("`usage: crane init <project name>`"));

            "It should display the description statement on the 3rd line"
              ._(() => result.Line(2).Should().Be("Initializes a new project with foo and bar."));

            "It should display the first line of an example as a title"
                ._(() => result.Lines().First(item => item.Contains("Example"))
                    .Should().StartWith("**")
                    .And.EndWith("**"));

        }
    }
}
