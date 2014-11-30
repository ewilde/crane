using System;
using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;

namespace Crane.Core.Templates.Parsers
{
    public class TokenTemplateParser : ITemplateParser
    {
        private readonly Dictionary<string, Func<string>> _tokens; 

        public TokenTemplateParser(ICraneContext context)
        {
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
            foreach (var token in _tokens)
            {
                if (template.Contains(token.Key))
                {
                    template = template.Replace(token.Key, token.Value.Invoke());
                }
            }

            return template;
        }
    }
}