using System.Reflection;
using Autofac;
using crane.core.Commands;
using Module = Autofac.Module;

namespace crane.console.Configuration.Modules
{
    public class CommandModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var commands = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(commands)
                .AssignableTo<ICraneCommand>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}