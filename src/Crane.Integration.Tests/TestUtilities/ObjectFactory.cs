using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autofac;
using Crane.Core.Configuration.Modules;
using Crane.Core.IO;
using log4net;

namespace Crane.Integration.Tests.TestUtilities
{
    public static class ioc
    {
        private static IContainer _container;

        private static readonly ILog _log = LogManager.GetLogger(typeof(Run));

        public static T Resolve<T>() where T : class
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
                catch (Exception exception)
                {
                    _log.ErrorFormat("Error trying to create an instance of {0}. {1}{2}", type.FullName, Environment.NewLine, exception.ToString());
                }
            }

            throw new Exception("No contructor was found that had all the dependencies satisfied.");
        }
    }
}