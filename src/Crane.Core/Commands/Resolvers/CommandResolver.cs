using System;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands.Exceptions;

namespace Crane.Core.Commands.Resolvers
{
    public class CommandResolver : ICommandResolver
    {
        
        public Type Resolve(IEnumerable<ICraneCommand> commands, string commandArgument)
        {
            
            var command = commands.FirstOrDefault(c => c.GetType().Name.ToLowerInvariant() == commandArgument.ToLowerInvariant());

            if (command == null)
                throw new UnknownCommandCraneException(string.Format("crane {0} is not a crane command. See 'crane listcommands'", commandArgument));

            return command.GetType();
        }
    }
}
