namespace Crane.Core.Commands.Factories
{
    public interface ICommandFactory
    {
        ICraneCommand Create(params string[] args);
    }
}