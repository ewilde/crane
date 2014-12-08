using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Commands.Resolvers;
using PowerArgs;

namespace Crane.Core.Commands.Handlers
{
    public abstract class CommandHandler<TCommand> where TCommand : class
    {
        public void Handle(ICraneCommand craneCommand)
        {
            var command = craneCommand as TCommand;
            DoHandle(command);
        }

        protected abstract void DoHandle(TCommand command);
    }

    public interface ICommandFactory
    {
        ICraneCommand Create(string[] args);
    }

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
            var command = _commandResolver.Resolve(_craneCommands, args[0]);
            var commandWithArgs = Args.Parse(command.GetType(), args.Skip(1).ToArray()) as ICraneCommand;
            return commandWithArgs;
        }
    }
}
