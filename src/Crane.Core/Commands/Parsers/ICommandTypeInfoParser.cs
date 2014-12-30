using System;
using System.Collections.Generic;
using Crane.Core.Commands.Arguments;

namespace Crane.Core.Commands.Parsers
{
    /// <summary>
    /// Returns meta-data for a given <see cref="ICraneCommand"/>
    /// </summary>
    public interface ICommandTypeInfoParser
    {
        IEnumerable<ICommandArgument> GetArguments(Type type);
    }
}