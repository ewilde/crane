using System;

namespace Crane.Core.Commands.Handlers
{
    public interface ICommandHandler
    {
        bool CanHandle(ICraneCommand command);
        void Handle(ICraneCommand craneCommand);
    }
}
