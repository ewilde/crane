using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands.Resolvers;

namespace Crane.Core.Commands.Execution
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IEnumerable<ICraneCommand> _commands;
        private readonly ICommandResolver _commandResolver;
        private readonly ICommandMethodResolver _commandMethodResolver;
        private readonly IDidYouMeanExecutor _didYouMeanExecutor;

        public CommandExecutor(IEnumerable<ICraneCommand> commands, 
                               ICommandResolver commandResolver, 
                                ICommandMethodResolver commandMethodResolver, IDidYouMeanExecutor didYouMeanExecutor)
        {
            _commands = commands;
            _commandResolver = commandResolver;
            _commandMethodResolver = commandMethodResolver;
            _didYouMeanExecutor = didYouMeanExecutor;
        }


        public void ExecuteCommand(string[] arguments)
        {
            var command = _commandResolver.Resolve(_commands, arguments[0]);
            var methodArgs = arguments.Skip(1).ToArray();
            var method = _commandMethodResolver.Resolve(command, methodArgs);

            if (method != null)
            {
                method.Invoke(command, methodArgs.Cast<object>().ToArray());
            }
            else
            {
                _didYouMeanExecutor.PrintHelp(command, arguments);    
            }
        }
    }
}