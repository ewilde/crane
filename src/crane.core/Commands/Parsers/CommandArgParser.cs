using System;
using System.Linq;
using Crane.Core.Commands.Exceptions;
using PowerArgs;

namespace Crane.Core.Commands.Parsers
{
    public class CommandArgParser : ICommandArgParser
    {
        public ICraneCommand Parse(Type commandType, string[] args)
        {
            try
            {
                var parsedCommand = Args.Parse(commandType, args.Skip(1).ToArray()) as ICraneCommand;
                return parsedCommand;
            }
            catch (ArgException argException)
            {
                throw new MissingArgumentCraneException(argException.Message);
            }
        }
    }
}