namespace Crane.Core.Commands.Arguments
{
    public interface ICommandArgument
    {
        string Name { get; }
        bool Required { get; }
    }
}