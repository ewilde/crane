using System.Collections;
using System.Collections.Generic;

namespace Crane.Core.Documentation
{
    public class CommandHelp : ICommandHelp
    {
        public CommandHelp(string commandName, string description, IEnumerable<CommandExample> examples)
        {
            CommandName = commandName;
            Description = description;
            Examples = examples;
        }

        public string CommandName { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<CommandExample> Examples { get; private set; }
    }
}