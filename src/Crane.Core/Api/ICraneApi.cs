using System.Collections;
using System.Collections.Generic;
using Crane.Core.Api.Model;
using Crane.Core.Runners;

namespace Crane.Core.Api
{
    public interface ICraneApi
    {
        ISolutionContext GetSolutionContext(string rootFolderPath);

        void PatchAssemblyInfo(AssemblyInfo assemblyInfo);

        void PatchSolutionAssemblyInfo(ISolutionContext solutionContext, string version);

        ISourceControlInformation GetSourceControlInformation(ISolutionContext solutionContext);
        
        IEnumerable<RunResult> NugetPublish(ISolutionContext solutionContext, string nugetOutputPath, string version, string source, string apiKey);
    }
}