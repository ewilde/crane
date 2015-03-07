using System;
using System.Collections.Generic;
using Crane.Core.Configuration;

namespace Crane.Core.Templates.Parsers
{
    public class TokenDictionaryFactory : ITokenDictionaryFactory
    {
        public ITokenDictionary Create(ICraneContext craneContext, IProjectContext projectContext)
        {
            return new TokenDictionary(new Dictionary<string, Func<string>>
            {
                { "%context.ProjectName%", () => projectContext.ProjectName},
                { "%context.SolutionPath%", () => projectContext.SolutionPath},
                { "%context.BuildDirectory.FullName%", () => craneContext.BuildDirectory.FullName},
                { "%context.CraneInstallDirectory.FullName%", () => craneContext.CraneInstallDirectory.FullName},
                { "%context.ProjectRootDirectory.FullName%", () => craneContext.ProjectRootDirectory.FullName},
                { "%context.Configuration.BuildFolderName%", () => craneContext.Configuration.BuildFolderName},
                { "%context.Configuration.BuildTemplateProviderName%", () => craneContext.Configuration.BuildTemplateProviderName},
                { "%DateTime.Now.Year%", () => DateTime.Now.Year.ToString()},
                { "%System.Environment.UserName%", () => System.Environment.UserName},
                {
                    "%nuget%", () => ".nuget" // cpack doesn't include folder name .nuGet so we just tokenize it
                },
            });
        }
    }
}