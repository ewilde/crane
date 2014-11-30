using System.Collections.Generic;

namespace Crane.Core.Commands.Resolvers
{

    public interface ICommandResolver
    {
        ICraneCommand Resolve(IEnumerable<ICraneCommand> commands, string commandArgument);
    }
}