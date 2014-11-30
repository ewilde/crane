using System.Reflection;
using Autofac;
using Crane.Core.Commands;
using Crane.Core.Templates;
using Module = Autofac.Module;

namespace Crane.Core.Configuration.Modules
{
    public class TemplateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var commands = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(commands)
                .AssignableTo<ITemplate>()
                .AsImplementedInterfaces();

            builder.RegisterType<RazorTemplateParser>().As<ITemplateParser>();
        }
    }
}