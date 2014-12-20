using System.Collections;
using System.Collections.Generic;

namespace Crane.Core.Documentation
{
    public interface ICommandHelp
    {
        string CommandName { get; }

        string Description { get; }

        IEnumerable<CommandExample> Examples { get; }
    }
}