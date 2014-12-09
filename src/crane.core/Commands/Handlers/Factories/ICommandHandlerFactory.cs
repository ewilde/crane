namespace Crane.Core.Commands.Handlers
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler Create(ICraneCommand command);
    }
}