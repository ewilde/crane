namespace Crane.Core.Commands.Handlers
{
    public interface ICommandFactory
    {
        ICraneCommand Create(string[] args);
    }
}