using System.Reflection;
using Autofac;
using Crane.Core.Commands;
using Module = Autofac.Module;


namespace Crane.Core.Configuration.Modules
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