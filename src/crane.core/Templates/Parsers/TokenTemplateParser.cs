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

        public TokenTemplateParser(ITokenDictionary tokenDictionary, IGuidGenerator guidGenerator)
        {
            _tokenDictionary = tokenDictionary;
            _guidGenerator = guidGenerator;
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
                template = template.Replace(string.Format("%GUID-{0}%", guidIndex), _guidGenerator.Create().ToString("B"));
                guidIndex += 1;
            }

            return template;
        }
    }
}