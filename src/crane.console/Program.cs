using Crane.Core.Hosts;

namespace Crane.Console
{
    public class Program
    {
        static int Main(string[] args)
        {
            var hostFactory = new HostFactory();
            var consoleHost = hostFactory.CreateConsoleHost();
            return consoleHost.Run(args);
        }

    }
}
