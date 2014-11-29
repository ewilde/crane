using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Module = Autofac.Module;

namespace Crane.Core.Configuration.Modules
{
    public class UnRegisteredTypesAsTransientModule 
    {
        private readonly Assembly[] _assemblies;

        public UnRegisteredTypesAsTransientModule(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public void AddUnregisteredTypesToContainer(IContainer container)
        {
            var containerBuilder = new ContainerBuilder();

            foreach (var assembly in _assemblies)
            {
                var typesWithInterfaces = assembly.GetTypes()
                    .Where(t => t.IsClass)
                    .Select(t => new
                    {
                        @Class = t,
                        @Interfaces = t.GetInterfaces()
                    }).ToList();

                foreach (var typeWithInterface in typesWithInterfaces)
                {
                    foreach (var typeInterface in typeWithInterface.Interfaces)
                    {
                        if (!container.IsRegistered(typeInterface))
                        {
                            containerBuilder.RegisterType(typeWithInterface.GetType())
                                .As(typeInterface)
                                .InstancePerDependency();
                        } 
                    }
                }


            }

            
            containerBuilder.Update(container);
        }
    }
}
