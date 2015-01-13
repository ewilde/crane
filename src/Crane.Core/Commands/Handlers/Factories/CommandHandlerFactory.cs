using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands.Exceptions;

namespace Crane.Core.Commands.Handlers.Factories
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        public CommandHandlerFactory(IEnumerable<ICommandHandler> commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        public ICommandHandler Create(ICraneCommand command)
        {
            var handler = _commandHandlers.FirstOrDefault(h => h.CanHandle(command));
            if (handler == null)
            {
                throw new MissingCommandHandlerException(string.Format("No handler found for command {0}.", command));
            }
            return handler;
        }
    }
}