using System;
using Xbehave;
using Xbehave.Fluent;
using Autofac;

namespace crane.core.tests.TestExtensions
{
    public static class ModuleExtension
    {
        public static IStep BuildContainerWithModule(this string text, Module module, out IContainer container)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            container = builder.Build();
            return text._(()=>{});
        }
    }
}