using System;
using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;

namespace Crane.Core.Templates.Parsers
{
    public class TokenTemplateParser : ITemplateParser
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly Dictionary<string, Func<string>> _tokens; 

        public TokenTemplateParser(ICraneContext context, IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
            _tokens = new Dictionary<string, Func<string>>
            {
                { "%context.ProjectName%", () => context.ProjectName},
                { "%context.BuildDirectory.FullName%", () => context.BuildDirectory.FullName},
                { "%context.CraneInstallDirectory.FullName%", () => context.CraneInstallDirectory.FullName},
                { "%context.ProjectRootDirectory.FullName%", () => context.ProjectRootDirectory.FullName},
                { "%context.Configuration.BuildFolderName%", () => context.Configuration.BuildFolderName},
                { "%context.Configuration.BuildTemplateProviderName%", () => context.Configuration.BuildTemplateProviderName},
            };
        }

        public string Parse(string template, object model)
        {
            template = ParseContextTokens(template);
            template = ParseGuidTokens(template);
            return template;
        }

        private string ParseContextTokens(string template)
        {
            foreach (var token in _tokens)
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