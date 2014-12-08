using System;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands.Resolvers;
using PowerArgs;

namespace Crane.Core.Commands.Handlers
{
    public abstract class CommandHandler<TCommand> : ICommandHandler where TCommand : class
    {
        public bool CanHandle(ICraneCommand command)
        {
            return command is TCommand;
        }

        public void Handle(ICraneCommand craneCommand)
        {
            var command = craneCommand as TCommand;
            DoHandle(command);
        }

        protected abstract void DoHandle(TCommand command);
    }

    public interface ICommandHandler
    {
        bool CanHandle(ICraneCommand command);
        void Handle(ICraneCommand craneCommand);
    }

    public interface ICommandFactory
    {
        ICraneCommand Create(string[] args);
    }

    public interface ICommandHandlerFactory
    {
        ICommandHandler Create(ICraneCommand command);
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
            var commandType = _commandResolver.Resolve(_craneCommands, args[0]);
            var commandWithArgs = Args.Parse(commandType, args.Skip(1).ToArray()) as ICraneCommand;
            return commandWithArgs;
        }
    }
}
