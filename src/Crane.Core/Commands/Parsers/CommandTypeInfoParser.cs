using System;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.Commands.Arguments;
using PowerArgs;

namespace Crane.Core.Commands.Parsers
{
    public class CommandTypeInfoParser : ICommandTypeInfoParser
    {
        public IEnumerable<ICommandArgument> GetArguments(Type type)
        {
            var argDefinition = new CommandLineArgumentsDefinition(type);
            return argDefinition.Arguments.Select(item=> new CommandArgument { Name = item.DefaultAlias, Required = item.IsRequired });
        }        
    }
}