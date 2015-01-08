using Autofac;
using Crane.Core.Commands;
using Crane.Core.Documentation;
using Crane.Core.Documentation.Formatters;
using Crane.Core.Documentation.Providers;

namespace Crane.Core.Configuration.Modules
{
    public class DocumentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<XmlHelpProvider>().As<IHelpProvider>().SingleInstance();            
            builder.RegisterType<ConsoleHelpFormatter>().As<IHelpFormatter>().SingleInstance();
            builder.RegisterType<MarkdownHelpFormatter>().SingleInstance();
        }
    }
}