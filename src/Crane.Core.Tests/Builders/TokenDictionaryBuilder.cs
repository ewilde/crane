using System;
using System.Collections.Generic;
using System.Linq;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Tests.Builders
{
    public class TokenDictionaryBuilder
    {        
        private readonly List<Tuple<string, string>>  _values;

        public TokenDictionaryBuilder()
        {
            _values = new List<Tuple<string, string>>();
        }


        public TokenDictionaryBuilder WithToken(string token, string value)
        {
            _values.Add(new Tuple<string, string>(token, value));
            return this;
        }

        public TokenDictionary Build()
        {
            var dictionary = _values.ToDictionary<Tuple<string, string>, string, Func<string>>(current => current.Item1, current => (() => current.Item2));

            return new TokenDictionary(dictionary);
        }
    }
   
}