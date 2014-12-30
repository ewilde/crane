using Autofac;

namespace Crane.Core.Configuration.Modules
{
    public class BootStrap
    {
        public static IContainer Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(BootStrap).Assembly);
            var container = builder.Build();
            var unRegisteredTypesAsTransientModule = new UnRegisteredTypesAsTransientModule();
            unRegisteredTypesAsTransientModule.AddUnregisteredTypesToContainer(container);

            return container;
        }
    }
}