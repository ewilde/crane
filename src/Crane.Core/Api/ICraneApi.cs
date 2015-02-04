using Crane.Core.Api.Model;

namespace Crane.Core.Api
{
    public interface ICraneApi
    {
        ISolutionContext GetSolutionContext(string rootFolderPath);

        void PatchAssemblyInfo(AssemblyInfo assemblyInfo);
        ISourceControlInformation GetSourceInformation(ISolutionContext solutionContext);
    }
}