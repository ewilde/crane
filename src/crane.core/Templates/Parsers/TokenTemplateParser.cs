using System;
using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;

namespace Crane.Core.Templates.Parsers
{
    public class TokenTemplateParser : ITemplateParser
    {
        private readonly ITokenDictionary _tokenDictionary;
        private readonly IGuidGenerator _guidGenerator;
        private Dictionary<string, Guid> _guidCache;
 
        public TokenTemplateParser(ITokenDictionary tokenDictionary, IGuidGenerator guidGenerator)
        {
            _tokenDictionary = tokenDictionary;
            _guidGenerator = guidGenerator;
            _guidCache = new Dictionary<string, Guid>();
        }

        public string Parse(string template, object model)
        {
            template = ParseContextTokens(template);
            template = ParseGuidTokens(template);
            return template;
        }

        private string ParseContextTokens(string template)
        {
            foreach (var token in _tokenDictionary.Tokens)
            {
                if (template.Contains(token.Key))
                {
                    template = template.Replace(token.Key, token.Value.Invoke());
                }
            }

            return template;
        }

        private string ParseGuidTokens(string template)
        {
            int guidIndex = 1;            
            while (template.Contains("%GUID-"))
            {
                var guidToken = string.Format("%GUID-{0}%", guidIndex);
                template = template.Replace(guidToken, GetGuid(guidToken));
                guidIndex += 1;
            }

            return template;
        }

        private string GetGuid(string key)
        {
            if (!_guidCache.ContainsKey(key))
            {
                _guidCache.Add(key, _guidGenerator.Create());
            }

            return _guidCache[key].ToString("B");
        }
    }
}