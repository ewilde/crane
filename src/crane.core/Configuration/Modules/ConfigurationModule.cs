using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Crane.Core.Configuration.Modules
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var commands = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(commands)
                .AssignableTo<IConfiguration>()
                .AsImplementedInterfaces();
        }
    }
}