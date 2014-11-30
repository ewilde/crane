using System.Collections.Generic;
using Autofac;
using Crane.Core.Commands;
using Crane.Core.Commands.Execution;
using Crane.Core.Commands.Resolvers;
using Crane.Core.Configuration.Modules;

namespace Crane.Console
{
    public class Program
    {
        private readonly ICommandExecutor _commandExecutor;

        static int Main(string[] args)
        {
            var container = BootStrap.Start();

            var program = new Program(container.Resolve<IEnumerable<ICraneCommand>>());
            return program.Run(args);
        }

        public Program(IEnumerable<ICraneCommand> commands)
        {
            _commandExecutor = new CommandExecutor(commands, 
                                                    new CommandResolver(), 
                                                    new CommandMethodResolver(), 
                                                    new DidYouMeanExecutor(new ClosestCommandMethodResolver(), System.Console.WriteLine));
        }

        public int Run(string [] args)
        {
            _commandExecutor.ExecuteCommand(args);
            return 0;
        }
    }
}
