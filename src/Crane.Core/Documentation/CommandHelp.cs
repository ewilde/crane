using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Crane.Core.Documentation
{
    public class CommandHelp : ICommandHelp
    {
        public CommandHelp(string commandName, string fullName, string description, IEnumerable<CommandExample> examples)
        {
            CommandName = commandName;
            CommandType = Type.GetType(fullName);
            FullName = fullName;
            Description = description;
            Examples = examples;
        }

        public string CommandName { get; private set; }

        public Type CommandType { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<CommandExample> Examples { get; private set; }
        
        public string FullName { get; set; }
    }
}