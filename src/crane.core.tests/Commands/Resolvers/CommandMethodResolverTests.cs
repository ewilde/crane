using System.Reflection;
using Crane.Core.Commands;
using Crane.Core.Commands.Resolvers;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Resolvers
{
    public class CommandMethodResolverTests
    {
       
        public class SingleCommandMethodScenarios
        {
            private class SingleMethodCommand : ICraneCommand
            {
                public string Name { get { return "SingleMethodCommand"; } }
                public void Execute() { }
            }

            [Scenario]
            public void Resolve_returns_Execute_when_no_arguments_given(CommandMethodResolver commandMethodResolver, MethodInfo result)
            {
                "Given I have a command method resolver"
                    ._(() => commandMethodResolver = new CommandMethodResolver());

                "When I call resolve with a single method command and no arguments"
                    ._(() => result = commandMethodResolver.Resolve(new SingleMethodCommand(), new string[0]));

                "Then it should resolve the execute method"
                    ._(() => result.Name.Should().Be("Execute"));
            }

            [Scenario]
            public void Resolve_returns_null_when_a_single_argument_given_and_there_are_no_matching_methods(CommandMethodResolver commandMethodResolver, MethodInfo result)
            {
                "Given I have a command method resolver"
                ._(() => commandMethodResolver = new CommandMethodResolver());

                "When I call resolve with a single method command and a single argument"
                    ._(() => result = commandMethodResolver.Resolve(new SingleMethodCommand(), new[] { "arg1" }));

                "Then it should return null"
                    ._(() => result.Should().BeNull());
            }
        }

        public class MultipleCommandMethodScenarios
        {

            private class MultipleMethodCommand : ICraneCommand
            {

                public void NoArgsMethod()
                {
                }

                public string Name { get { return "MultipleMethodCommand"; } }
                public void OneArgMethod(string arg1)
                {
                }

                public void TwoArgsMethod(string arg1, string arg2)
                {
                }

                public void ThreeArgsMethod(string arg1, string arg2, string arg3)
                {
                }

            }

            [Scenario]
            public void Resolve_returns_correct_method_when_no_arguments_given(CommandMethodResolver commandMethodResolver,
                MethodInfo result)
            {
                "Given I have a command method resolver"
                    ._(() => commandMethodResolver = new CommandMethodResolver());

                "When I call resolve with a multiple method command and no arguments"
                    ._(() => result = commandMethodResolver.Resolve(new MultipleMethodCommand(), new string[0]));

                "Then it should resolve the NoArgsMethod"
                    ._(() => result.Name.Should().Be("NoArgsMethod"));
            }

            [Scenario]
            public void Resolve_returns_correct_method_when_one_argument_given(CommandMethodResolver commandMethodResolver,
                MethodInfo result)
            {
                "Given I have a command method resolver"
                    ._(() => commandMethodResolver = new CommandMethodResolver());

                "When I call resolve with a multiple method command and one argument"
                    ._(() => result = commandMethodResolver.Resolve(new MultipleMethodCommand(), new []{ "firstarg"}));

                "Then it should resolve the OneArgMethod"
                    ._(() => result.Name.Should().Be("OneArgMethod"));
            }

            [Scenario]
            public void Resolve_returns_correct_method_when_two_arguments_given(CommandMethodResolver commandMethodResolver,
                MethodInfo result)
            {
                "Given I have a command method resolver"
                    ._(() => commandMethodResolver = new CommandMethodResolver());

                "When I call resolve with a multiple method command and two arguments"
                    ._(() => result = commandMethodResolver.Resolve(new MultipleMethodCommand(), new[] { "firstarg", "secondarg" }));

                "Then it should resolve the TwoArgsMethod"
                    ._(() => result.Name.Should().Be("TwoArgsMethod"));
            }

            [Scenario]
            public void Resolve_returns_correct_method_when_three_arguments_given(CommandMethodResolver commandMethodResolver,
                MethodInfo result)
            {
                "Given I have a command method resolver"
                    ._(() => commandMethodResolver = new CommandMethodResolver());

                "When I call resolve with a multiple method command and three arguments"
                    ._(() => result = commandMethodResolver.Resolve(new MultipleMethodCommand(), new[] { "firstarg", "secondarg", "thirdarg" }));

                "Then it should resolve the ThreeArgsMethod"
                    ._(() => result.Name.Should().Be("ThreeArgsMethod"));
            }

            [Scenario]
            public void Resolve_returns_null_when_four_arguments_given(CommandMethodResolver commandMethodResolver,
                MethodInfo result)
            {
                "Given I have a command method resolver"
                    ._(() => commandMethodResolver = new CommandMethodResolver());

                "When I call resolve with a multiple method command and four arguments"
                    ._(() => result = commandMethodResolver.Resolve(new MultipleMethodCommand(), new[] { "firstarg", "secondarg", "thirdarg", "fourtharg" }));

                "Then it should resolve null"
                    ._(() => result.Should().BeNull());
            }
        }

        public class IgnoresAutoPropertiesScenarios
        {
            private class TestCommandWithAutoProperty : ICraneCommand
            {
                public string MyProperty { get; set; }

                public void Execute() {}

                public string Name { get { return "TestCommandWithAutoProperty"; } }
            }

            [Scenario]
            public void Resolve_does_not_return_auto_properties(CommandMethodResolver commandMethodResolver,
                MethodInfo result)
            {
                "Given I have a command method resolver"
                    ._(() => commandMethodResolver = new CommandMethodResolver());

                "When I call resolve with a multiple method command and four arguments"
                    ._(() => result = commandMethodResolver.Resolve(new TestCommandWithAutoProperty(), new string[0]));

                "Then it should resolve the Execute command"
                    ._(() => result.Name.Should().Be("Execute"));
            }
        }

    }
}
