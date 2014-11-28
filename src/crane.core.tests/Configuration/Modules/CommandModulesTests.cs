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
        public void command_module_registration(IContainer container, CommandModule module)
        {
            "Given we have a command module"
                ._(() => module = new CommandModule());

            "When I build the module"
                ._(() => container = module.BuildContainerWithModule());

            "Then it should resolve the help command" 
                ._(() => container.Resolve<IEnumerable<ICraneCommand>>().Any(item => item is ShowHelp).Should().BeTrue());

            "And it should be a singleton instance" // Is there a better way to verify lifecycle in Autofac?
                ._(
                    () =>
                        ReferenceEquals(container.Resolve<IEnumerable<ICraneCommand>>().First(item => item is ShowHelp),
                            container.Resolve<IEnumerable<ICraneCommand>>().First(item => item is ShowHelp))
                            .Should().BeTrue());
        }
    }
}