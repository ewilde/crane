using System.Linq;
using Crane.Core.Commands;
using Crane.Core.Documentation;
using Crane.Core.Documentation.Parsers;
using Crane.Core.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Documentation.Parsers
{
    public class XmlCommentCommandHelpParserTests
    {
        [Scenario]
        public void can_parse_xml_comment_file_and_generate_command_help(string documentation, ICommandHelpParser parser, ICommandHelpCollection result)
        {
            "Given I have an xml comment file"
                ._(() => documentation = @"<?xml version=""1.0""?>
                    <doc>
                        <assembly>
                            <name>Crane.Core</name>
                        </assembly>
                        <members>
                            <member name=""T:Crane.Core.Commands.Init"">
                                <summary>
                                Initializes a new project
                                </summary>
                                <example>
                                EXAMPLE 1
                                <code>usage: crane init SallyFx</code>
                                This example initializes a new project 'SallyFx' in the current directory
                                </example>
                            </member>
                            <member name=""T:Crane.Core.Commands.Help"">
                                <summary>
                                Displays help for crane commands
                                </summary>            
                            </member>
                        </members>
                    </doc>
                    ");

            "And I have a command help parser"
                ._(() => parser = ioc.Resolve<XmlCommentCommandHelpParser>());

            "When I parse the comment file"
                ._(() => result = parser.Parse(documentation));

            "Then it should return a command help collection"
                ._(() => result.Should().NotBeNull());

            "And it should have help for each command referenced in the comment file"
                ._(() => result.Count.Should().Be(2));

            "And it should contain parsed command help"
                ._(() => result.Get("init").Should().NotBeNull());

            "And it should have parsed the description"
                ._(() => result.Get("init").Description.Should().Be("Initializes a new project"));

            "And it should have parsed the example"
                ._(() => result.Get("init").Examples.First().Value.Should().Contain("This example initializes a new project 'SallyFx' in the current directory"));
        }
    }
}