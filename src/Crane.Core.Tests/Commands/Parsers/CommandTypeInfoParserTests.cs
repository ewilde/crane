using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands;
using Crane.Core.Commands.Arguments;
using Crane.Core.Commands.Parsers;
using Crane.Core.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Parsers
{
    public class CommandTypeInfoParserTests
    {
        [Scenario]
        public void can_retrieve_command_arguments_from_a_type_with_required_arguments(ICommandTypeInfoParser infoParser, IEnumerable<ICommandArgument> result)
        {
            "Give I have a command type info parser"
                ._(() => infoParser = ioc.Resolve<CommandTypeInfoParser>());

            "When I parse the arguments"
                ._(() => result = infoParser.GetArguments(typeof(Init)));

            "It should retreive them"
                ._(() => result.Should().NotBeNull());

            "It should have the correct argument name"
                ._(() => result.First().Name.Should().Be("ProjectName"));

            "It should have the correct number of required arguments"
                ._(() => result.First().Required.Should().BeTrue());
        }
    }
}