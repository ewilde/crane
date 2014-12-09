using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Commands.Resolvers;
using PowerArgs;

namespace Crane.Core.Commands.Factories
{
    public class CommandFactory : ICommandFactory
    {
        private readonly ICommandResolver _commandResolver;
        private readonly IEnumerable<ICraneCommand> _craneCommands; 

        public CommandFactory(ICommandResolver commandResolver, IEnumerable<ICraneCommand> craneCommands)
        {
            _commandResolver = commandResolver;
            _craneCommands = craneCommands;
        }

        public ICraneCommand Create(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return new ListCommands();

                var commandType = _commandResolver.Resolve(_craneCommands, args[0]);
                var commandWithArgs = Args.Parse(commandType, args.Skip(1).ToArray()) as ICraneCommand;
                return commandWithArgs;
            }
            catch (ArgException argException)
            {
                throw new MissingArgumentCraneException(argException.Message);
            }
        }
    }
}