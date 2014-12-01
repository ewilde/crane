using System.Linq;
using System.Reflection;
using Autofac;

namespace Crane.Core.Configuration.Modules
{
    public class UnRegisteredTypesAsTransientModule 
    {
        private readonly Assembly[] _assemblies;

        public UnRegisteredTypesAsTransientModule() : this(Assembly.GetExecutingAssembly())
        {
            
        }

        public UnRegisteredTypesAsTransientModule(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public void AddUnregisteredTypesToContainer(IContainer container)
        {
            var containerBuilder = new ContainerBuilder();

            foreach (var assembly in _assemblies)
            {

                foreach (var type in assembly.GetTypes()
                                         .Where(t => t.IsInterface))
                {
                    if (!container.IsRegistered(type))
                    {
                        containerBuilder.RegisterAssemblyTypes(assembly)
                                .AssignableTo(type)
                                .AsImplementedInterfaces()
                                .InstancePerDependency();
                    }
                }
            }

            
            containerBuilder.Update(container);
        }
    }
}
