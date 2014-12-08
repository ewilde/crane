using System.Reflection;
using Crane.Core.Commands;
using Crane.Core.Commands.Resolvers;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Commands.Resolvers
{
    public class ClosestCommandMethodResolverTests
    {
        private class SingleMethodCommandWithNoArguments : ICraneCommand
        {
            public void Execute() { }
        }

        [Scenario]
        public void Resolve_returns_method_with_fewest_parameters_when_arguments_given(ClosestCommandMethodResolver closestCommandMethodResolver, MethodInfo result)
        {
            "Given I have a closted command method resolver"
                ._(() => closestCommandMethodResolver = new ClosestCommandMethodResolver());

            "When I call resolve a method with 1 argument"
                ._(() => result = closestCommandMethodResolver.Resolve(new SingleMethodCommandWithNoArguments(), new [] {"arg1"}));

            "Then it should resolve the execute method with no arguments"
                ._(() => result.Name.Should().Be("Execute"));
        }


        private class MultipleMethodCommandWithArguments : ICraneCommand
        {
            public void OneArgument(string arg) { }
            public void TwoArguments(string arg1, string arg2) { }
        }

        [Scenario]
        public void Resolve_returns_method_with_fewest_parameters_when_no_arguments_given(ClosestCommandMethodResolver closestCommandMethodResolver, MethodInfo result)
        {
            "Given I have a cloest command method resolver"
            ._(() => closestCommandMethodResolver = new ClosestCommandMethodResolver());

            "When I call resolve with a method passing in no arguments"
                ._(() => result = closestCommandMethodResolver.Resolve(new MultipleMethodCommandWithArguments(), new string[0]));

            "Then it should return the OneArgument method"
                ._(() => result.Name.Should().Be("OneArgument"));
        }
    }
}
