using System;
using System.Collections.Generic;
using System.Linq;

namespace Crane.Core.Commands.Resolvers
{
    public class CommandResolver : ICommandResolver
    {
        public Type Resolve(IEnumerable<ICraneCommand> commands, string commandArgument)
        {
            
            var command = commands.FirstOrDefault(c => c.Name.ToLowerInvariant() == commandArgument.ToLowerInvariant());

            if (command == null)
                return typeof (Help);

            return command.GetType();
        }
    }
}
