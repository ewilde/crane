using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands;
using Crane.Core.Commands.Attributes;
using Crane.Core.Commands.Resolvers;
using Crane.Core.Tests.TestUtilities;
using FluentAssertions;
using FluentAssertions.Common;
using Xbehave;

namespace Crane.Core.Tests.Commands.Resolvers
{
    public class VisibleCommandResolverTests
    {
        [Scenario]
        public void only_returns_commands_that_dont_have_the_hidden_attribute(VisibleCommandResolver commandResolver, IEnumerable<ICraneCommand> result)
        {
            "Given I have a command resolver"
                ._(() => commandResolver = new VisibleCommandResolver(new ICraneCommand[] { new VisableCommand(), new HiddenCommand()}));

            "When I resolve all the visible commands"
                ._(() => result = commandResolver.Resolve());

            "Then I should only have visible commands"
                ._(() => result.All(item => !item.GetType().HasAttribute<HiddenCommandAttribute>()).Should().BeTrue());
        }


        public class VisableCommand : ICraneCommand { }

        [Crane.Core.Commands.Attributes.HiddenCommand]
        public class HiddenCommand : ICraneCommand { }
    }
}