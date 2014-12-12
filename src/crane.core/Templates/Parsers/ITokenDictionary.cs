using System;
using System.Collections.Generic;

namespace Crane.Core.Templates.Parsers
{
    public interface ITokenDictionary
    {
        Dictionary<string, Func<string>> Tokens { get; }
    }
}