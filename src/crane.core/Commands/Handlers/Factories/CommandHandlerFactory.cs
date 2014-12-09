using System.Collections.Generic;
using System.Linq;

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
            var handler = _commandHandlers.First(h => h.CanHandle(command));
            return handler;
        }
    }
}