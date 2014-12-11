using System;
using Crane.Core.Commands;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Commands.Parsers;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using PowerArgs;
using Xbehave;

namespace Crane.Core.Tests.Commands.Parsers
{
    public class CommandArgParserTests
    {
        public class FooCommand : ICraneCommand
        {
            [ArgRequired]
            [ArgPosition(0)]
            public string Bar { get; set; }

            public string Baz { get; set; }
        }

        [Scenario]
        public void Parses_args_when_passed_by_position(CommandArgParser commandArgParser, FooCommand result)
        {
            "Given I have a commandArgParser"
                ._(() => commandArgParser = new CommandArgParser());

            "When I parse the command 'FooCommand Hello'"
                ._(() => result = commandArgParser.Parse(typeof (FooCommand), new[] {"FooCommand", "Hello"}) as FooCommand);

            "Then the result should have Bar set to Hello"
                ._(() => result.Bar.Should().Be("Hello"));

            "And the result should not have Baz set"
                ._(() => result.Baz.Should().BeNull());

        }

        [Scenario]
        public void Parses_args_when_passed_by_switch(CommandArgParser commandArgParser, FooCommand result)
        {
            "Given I have a commandArgParser"
                ._(() => commandArgParser = new CommandArgParser());

            "When I parse the command 'FooCommand -Bar Hello'"
                ._(() => result = commandArgParser.Parse(typeof(FooCommand), new[] { "FooCommand", "-Bar", "Hello" }) as FooCommand);

            "Then the result should have Bar set to Hello"
                ._(() => result.Bar.Should().Be("Hello"));

            "And the result should not have Baz set"
                ._(() => result.Baz.Should().BeNull());

        }

        [Scenario]
        public void Parses_all_args_when_passed_by_mixture_of_switch_and_position(CommandArgParser commandArgParser, FooCommand result)
        {
            "Given I have a commandArgParser"
                ._(() => commandArgParser = new CommandArgParser());

            "When I parse the command 'FooCommand Hello -Baz World'"
                ._(() => result = commandArgParser.Parse(typeof(FooCommand), new[] { "FooCommand", "Hello", "-Baz", "World" }) as FooCommand);

            "Then the result should have Bar set to Hello"
                ._(() => result.Bar.Should().Be("Hello"));

            "And the result should have Baz set to World"
                ._(() => result.Baz.Should().Be("World"));

        }

        [Scenario]
        public void Parses_all_args_when_all_passed_by_switch(CommandArgParser commandArgParser, FooCommand result)
        {
            "Given I have a commandArgParser"
                ._(() => commandArgParser = new CommandArgParser());

            "When I parse the command 'FooCommand -Bar Hello -Baz World'"
                ._(() => result = commandArgParser.Parse(typeof(FooCommand), new[] { "FooCommand", "-Bar", "Hello", "-Baz", "World" }) as FooCommand);

            "Then the result should have Bar set to Hello"
                ._(() => result.Bar.Should().Be("Hello"));

            "And the result should have Baz set to World"
                ._(() => result.Baz.Should().Be("World"));

        }

        [Scenario]
        public void Throws_MissingArgumentCraneException_when_required_parameter_not_given(CommandArgParser commandArgParser, Exception result)
        {
            "Given I have a commandArgParser"
                ._(() => commandArgParser = new CommandArgParser());

            "When I dont pass in the Bar parameter"
                ._(() => result = Throws.Exception(() => commandArgParser.Parse(typeof (FooCommand), new[] {"FooCommand"})));

            "Then a MissingArgumentCraneException is thrown"
                ._(() => result.Should().BeOfType<MissingArgumentCraneException>());

        }
    }
}
