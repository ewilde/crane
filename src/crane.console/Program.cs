using System.Collections.Generic;
using Crane.Core.Commands;
using Crane.Core.Commands.Execution;
using Crane.Core.Commands.Resolvers;

namespace Crane.Console
{
    public class Program
    {
        private readonly ICommandExecutor _commandExecutor;

        static int Main(string[] args)
        {
            var program = new Program();
            return program.Run(args);
        }

        public Program()
        {
            var commands = new List<ICraneCommand> {new Init(), new Help()};
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
