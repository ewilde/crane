using System;
using System.Collections.Generic;
using System.Linq;

namespace Crane.Core.Commands.Resolvers
{
    public class CommandResolver : ICommandResolver
    {
        private readonly Type _default = typeof (Help);
        public Type Resolve(IEnumerable<ICraneCommand> commands, string commandArgument)
        {
            
            var command = commands.FirstOrDefault(c => c.GetType().Name.ToLowerInvariant() == commandArgument.ToLowerInvariant());

            if (command == null)
                return _default;

            return command.GetType();
        }
    }
}
