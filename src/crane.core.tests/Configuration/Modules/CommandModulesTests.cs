using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Autofac;
using crane.console.Configuration.Modules;
using crane.core.Commands;
using crane.core.tests.TestExtensions;
using FluentAssertions;
using Xbehave;

namespace crane.core.tests.Configuration.Modules
{
    public class CommandModulesTests
    {
        [Scenario]
        public void command_module_registration(IContainer container, CommandModule module)
        {
            "Given we have a command modlue"
                ._(() => module = new CommandModule());

            "When I build the module"
                ._(() => container = module.BuildContainerWithModule());

            "Then it should resolve the help command"
                ._(() => container.Resolve<IEnumerable<ICraneCommand>>().Any(item => item is ShowHelp).Should().BeTrue());
        }
    }
}