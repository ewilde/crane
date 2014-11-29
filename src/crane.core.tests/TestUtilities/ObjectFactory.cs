using System;
using System.Collections.Generic;
using Autofac;
using Crane.Core.Configuration.Modules;

namespace Crane.Integration.Tests.TestUtilities
{
    public static class a
    {
        private static IContainer _container;

        public static T New<T>() where T : class
        {
            if (_container == null)
            {
                _container = BootStrap.Start();
            }

            if (_container.IsRegistered<T>())
            {
                return _container.Resolve<T>();
            }

            return ResolveUnregistered(_container, typeof(T)) as T;
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