using System.Collections.Generic;
using Crane.Core.Commands.Parsers;
using Crane.Core.Commands.Resolvers;

namespace Crane.Core.Commands.Factories
{
    public class CommandFactory : ICommandFactory
    {
        private readonly ICommandResolver _commandResolver;
        private readonly ICommandArgParser _commandArgParser;
        private readonly IEnumerable<ICraneCommand> _craneCommands; 

        public CommandFactory(ICommandResolver commandResolver, 
                                ICommandArgParser commandArgParser,
                                IEnumerable<ICraneCommand> craneCommands)
        {
            _commandResolver = commandResolver;
            _craneCommands = craneCommands;
            _commandArgParser = commandArgParser;
        }

        public ICraneCommand Create(string[] args)
        {
            if (args == null || args.Length == 0)
                return new ListCommands();

            var commandType = _commandResolver.Resolve(_craneCommands, args[0]);
            var commandWithArgs = _commandArgParser.Parse(commandType, args);
            return commandWithArgs;   
        }
    }
}