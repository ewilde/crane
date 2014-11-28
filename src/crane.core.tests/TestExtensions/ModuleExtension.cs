using Autofac;

namespace Crane.Core.Tests.TestExtensions
{
    public static class ModuleExtension
    {
        public static IContainer BuildContainerWithModule(this Module module)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();
            return container;
        }
    }
}