namespace Crane.Core.Commands.Handlers.Factories
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler Create(ICraneCommand command);
    }
}