using Autofac;
using Crane.Core.Api;

namespace Crane.Core.Configuration.Modules
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(ICraneApi).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<ICraneApi>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}