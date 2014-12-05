using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Crane.Core.Commands.Resolvers
{
    public class CommandResolver : ICommandResolver
    {
        public ICraneCommand Resolve(IEnumerable<ICraneCommand> commands, string commandArgument)
        {
            
            var command = commands.FirstOrDefault(c => c.Name.ToLowerInvariant() == commandArgument.ToLowerInvariant());

            if (command == null)
                return new Help();

            return command;
        }
    }
}
