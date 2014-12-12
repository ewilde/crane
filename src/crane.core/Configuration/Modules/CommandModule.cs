﻿using Autofac;
using Crane.Core.Commands;
using Module = Autofac.Module;


namespace Crane.Core.Configuration.Modules
{
    public class CommandModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof (ICraneCommand).Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .AssignableTo<ICraneCommand>()
                .AsImplementedInterfaces()
                .SingleInstance();

            
        }
    }


}