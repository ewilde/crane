using System;
using System.Collections.Generic;

namespace Crane.Core.Commands.Resolvers
{

    public interface ICommandResolver
    {
        Type Resolve(IEnumerable<ICraneCommand> commands, string commandArgument);
    }
}