using Autofac;
using Autofac.Core;
using Crane.Core.Configuration.Modules;
using FluentAssertions;
using Xbehave;

namespace Crane.Core.Tests.Configuration.Modules
{
    public class UnRegisteredTypesAsTransientModuleTests
    {
        public interface IFoo {}

        public class Bar : IFoo {}

        public interface IOther {}

        public class Other : IOther {}

        [Scenario]
        public void Unregistered_types_get_registered_as_transient(IContainer container,
            UnRegisteredTypesAsTransientModule module)
        {
            "Given I have registered the type Other as IOther as singleton"
                ._(() =>
                {
                    module = new UnRegisteredTypesAsTransientModule(typeof(IFoo).Assembly);
                    var containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterType<Other>().As<IOther>().SingleInstance();
                    container = containerBuilder.Build();
                });

            "When I install the module"
                ._(() => module.AddUnregisteredTypesToContainer(container));

            "Then Bar should be registered as IFoo"
                ._(() => container.Resolve<IFoo>().GetType().Should().Be(typeof(Bar)));

            "And IFoo should be transient"
                ._(() => ReferenceEquals(container.Resolve<IFoo>(), container.Resolve<IFoo>()).Should().BeFalse());

            "And IOther should still be registered as singleton"
                ._(() => ReferenceEquals(container.Resolve<IOther>(), container.Resolve<IOther>()).Should().BeTrue());

        }
    }
}