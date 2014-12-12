using Autofac;
using Crane.Core.Configuration.Modules;

namespace Crane.Core.Hosts
{
    public class HostFactory
    {        
        public IHost CreateConsoleHost()
        {
            var container = BootStrap.Start();
            return container.Resolve<IHost>();
        }
    }
}
