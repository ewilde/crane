using System.Reflection;
using Autofac;
using Crane.Core.Commands;
using Crane.Core.Templates;
using Crane.Core.Templates.Parsers;
using Crane.Core.Templates.Resolvers;
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

            builder.RegisterType<TokenTemplateParser>().As<ITemplateParser>();
            builder.RegisterType<TemplateResolver>().As<ITemplateResolver>();
        }
    }
}