using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Configuration;

namespace Crane.Core.Templates.Parsers
{
    public class TokenDictionary : ITokenDictionary
    {
        private readonly Dictionary<string, Func<string>> _tokens;

        public TokenDictionary(ICraneContext context)
        {
            _tokens = new Dictionary<string, Func<string>>
            {
                { "%context.ProjectName%", () => context.ProjectName},
                { "%context.BuildDirectory.FullName%", () => context.BuildDirectory.FullName},
                { "%context.CraneInstallDirectory.FullName%", () => context.CraneInstallDirectory.FullName},
                { "%context.ProjectRootDirectory.FullName%", () => context.ProjectRootDirectory.FullName},
                { "%context.Configuration.BuildFolderName%", () => context.Configuration.BuildFolderName},
                { "%context.Configuration.BuildTemplateProviderName%", () => context.Configuration.BuildTemplateProviderName},
                { "%DateTime.Now.Year%", () => DateTime.Now.Year.ToString()},
            };
        }

        public Dictionary<string, Func<string>> Tokens
        {
            get { return _tokens; }
        }
    }
}
