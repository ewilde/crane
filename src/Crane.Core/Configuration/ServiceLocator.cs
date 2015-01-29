using System;
using System.Collections.Generic;
using Autofac;
using Crane.Core.Api;
using Crane.Core.Configuration.Modules;

namespace Crane.Core.Configuration
{
    /// <summary>
    /// In the powershell cmdlet, havn't figured out a way to wire up
    /// the ioc engine, so using ServiceLocator until we do.
    /// </summary>
    public static class ServiceLocator
    {
        private static IContainer _container;

        private static IContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = BootStrap.Start();
                }

                return _container;
            }
        }

        public static T BuildUp<T>(T item)
        {
            return Container.InjectProperties(item);
        }

        public static T Resolve<T>() where T : class
        {
            
            if (Container.IsRegistered<T>())
            {
                return Container.Resolve<T>();
            }

            return ResolveUnregistered(Container, typeof(T)) as T;
        }

        public static object ResolveUnregistered(IContainer container, Type type)
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = container.Resolve(parameter.ParameterType);
                        if (service == null) throw new Exception("Unkown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {

                }
            }
            throw new Exception("No contructor was found that had all the dependencies satisfied.");
        }
    }
}