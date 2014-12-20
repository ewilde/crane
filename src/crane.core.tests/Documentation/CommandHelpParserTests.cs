using System.Collections.Generic;
using Crane.Core.Commands;
using Crane.Core.Documentation;
using Crane.Core.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Documentation
{
    public class CommandHelpParserTests
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
                                <code>
                                    crane init SallyFx
                                </code>
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
                ._(() => parser = ioc.Resolve<CommandHelpParser>());

            "When I parse the comment file"
                ._(() => result = parser.Parse(documentation));

            "It should return a command help collection"
                ._(() => result.Should().NotBeNull());

            "It should have help for each command referenced in the comment file"
                ._(() => result.Count.Should().Be(2));

            "It should contain parsed command help"
                ._(() => result.Get<Init>().Should().NotBeNull());
        }
    }
}