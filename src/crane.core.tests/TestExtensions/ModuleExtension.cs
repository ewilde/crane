using System;
using Xbehave;
using Xbehave.Fluent;
using Autofac;

namespace crane.core.tests.TestExtensions
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