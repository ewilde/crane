using Autofac;
using Crane.Core.IO;

namespace Crane.Core.Configuration.Modules
{
    public class IoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileManager>().As<IFileManager>().SingleInstance();
        }
    }
}