using Autofac;
using Crane.Core.Templates;
using Crane.Core.Templates.Parsers;
using Module = Autofac.Module;

namespace Crane.Core.Configuration.Modules
{
    public class TemplateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof (ITemplate).Assembly;
            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<ITemplate>()
                .AsImplementedInterfaces();

            builder.RegisterType<TokenDictionary>().As<ITokenDictionary>().SingleInstance();
            builder.RegisterType<TokenTemplateParser>().As<ITemplateParser>();
        }
    }
}