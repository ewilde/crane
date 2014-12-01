using Autofac;
using Crane.Core.Configuration.Modules;

namespace Crane.Core.Hosts
{
    public class HostFactory
    {

        private IContainer CreateContainer<THost>() where THost : IHost
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new CommandModule());
            containerBuilder.RegisterType<THost>()
                            .As<IHost>()
                            .SingleInstance();

            var container = containerBuilder.Build();
            var unRegisteredTypesAsTransientModule = new UnRegisteredTypesAsTransientModule();
            unRegisteredTypesAsTransientModule.AddUnregisteredTypesToContainer(container);

            return container;
        }
        public IHost CreateConsoleHost()
        {
            var container = CreateContainer<ConsoleHost>();
            return container.Resolve<IHost>();
        }
    }
}
