using Crane.Core.Commands;

namespace Crane.Core.Documentation
{
    public interface ICommandHelpCollection
    {
        ICommandHelp Get<T>() where T : ICraneCommand;
        int Count { get; }
    }
}