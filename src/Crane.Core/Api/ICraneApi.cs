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
        RunResult NugetPublish(ISolutionContext solutionContext);
    }
}