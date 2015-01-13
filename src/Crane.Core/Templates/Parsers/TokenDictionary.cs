using System;
using System.Collections.Generic;

namespace Crane.Core.Templates.Parsers
{
    public class TokenDictionary : ITokenDictionary
    {
        private readonly Dictionary<string, Func<string>> _tokens;

        public TokenDictionary(Dictionary<string, Func<string>> tokens)
        {
            _tokens = tokens;
        }

        public Dictionary<string, Func<string>> Tokens
        {
            get { return _tokens; }
        }
    }
}
