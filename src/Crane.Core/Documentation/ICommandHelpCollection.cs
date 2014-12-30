using Crane.Core.Commands;

namespace Crane.Core.Documentation
{
    public interface ICommandHelpCollection
    {       
        ICommandHelp Get(string command);

        int Count { get; }
    }
}