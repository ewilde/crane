using System;

namespace Crane.Core.Commands.Parsers
{
    public interface ICommandArgParser
    {
        ICraneCommand Parse(Type commandType, params string[] args);
    }
}