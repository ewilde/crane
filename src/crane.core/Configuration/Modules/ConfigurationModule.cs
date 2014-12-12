using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace Crane.Core.Configuration.Modules
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CraneContext>().As<ICraneContext>().SingleInstance();
            builder.RegisterType<CraneConfiguration>().As<IConfiguration>().SingleInstance();
        }
    }
}