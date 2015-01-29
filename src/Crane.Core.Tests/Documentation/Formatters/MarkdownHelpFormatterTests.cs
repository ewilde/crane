using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Configuration;
using Crane.Core.Documentation;
using Crane.Core.Documentation.Formatters;
using Crane.Core.Extensions;
using Crane.Core.Tests.TestUtilities;
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
                ._(() => formatter = ServiceLocator.Resolve<MarkdownHelpFormatter>());

            "And I have a help command"
                ._(() => commandHelp = new CommandHelp("init", "Crane.Core.Commands.Init", 
                    "Initializes a new project with foo and bar.", 
                    new[] { new CommandExample{ Value = "Example 1\n<code>usage</code>\n<code>\nvar x = 1;\nvar y=2;\n</code>"} }));

            "When I format the command"
                ._(() => result = formatter.Format(commandHelp));

            "Then it should display the usage statement on the first line"
                ._(() => result.Line(0).Should().Be("`usage: crane init <project name>`"));

            "And it should display the description statement on the 3rd line"
              ._(() => result.Line(2).Should().Be("Initializes a new project with foo and bar."));

            "And it should display the first line of an example as a title"
                ._(() => result.Lines().First(item => item.Contains("Example"))
                    .Should().StartWith("**")
                    .And.EndWith("**  "));

            "And it should display any code element values that start and begin on the same line as markdown code blocks using single back ticks"
                ._(() => result.Should().Contain("`usage`"));

            "And it should display any multiline code blocks as markdown code blocks"
                ._(
                    () =>
                        result.Should().Contain(string.Format("```{0}var x = 1;{0}var y=2;{0}```", Environment.NewLine)));
        }
    }
}
