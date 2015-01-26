using System.Collections.Generic;
using System.Linq;
using Autofac;
using Crane.Core.Api;
using Crane.Core.Commands;
using Crane.Core.Configuration.Modules;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Configuration.Modules
{
    public class ApiModuleTests
    {
        [Scenario]
        public void api_module_registration(IContainer container)
        {
            "Given we have bootstraped the IoC"
                ._(() => container = BootStrap.Start());

            "Then it should resolve the crane api"
                ._(() => container.Resolve<IEnumerable<ICraneApi>>().Any(item => item.GetType() == typeof(CraneApi)).Should().BeTrue());

            "And it should be a singleton instance" // Is there a better way to verify lifecycle in Autofac?
                ._(
                    () =>
                        ReferenceEquals(container.Resolve<ICraneApi>(),
                            container.Resolve<ICraneApi>())
                            .Should().BeTrue());
        } 
    }
}