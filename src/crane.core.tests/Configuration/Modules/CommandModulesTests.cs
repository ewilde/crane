using System.Collections.Generic;
using System.Linq;
using Autofac;
using Crane.Core.Commands;
using Crane.Core.Configuration.Modules;
using Crane.Core.Tests.TestExtensions;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Configuration.Modules
{
    public class CommandModulesTests
    {
        [Scenario]
        public void command_module_registration(IContainer container)
        {
            "Given we have bootstraped the IoC"
                ._(() => container = BootStrap.Start());

            "Then it should resolve the help command" 
                ._(() => container.Resolve<IEnumerable<ICraneCommand>>().Any(item => item is Help).Should().BeTrue());

            "And it should be a singleton instance" // Is there a better way to verify lifecycle in Autofac?
                ._(
                    () =>
                        ReferenceEquals(container.Resolve<IEnumerable<ICraneCommand>>().First(item => item is Help),
                            container.Resolve<IEnumerable<ICraneCommand>>().First(item => item is Help))
                            .Should().BeTrue());
        }
    }
}