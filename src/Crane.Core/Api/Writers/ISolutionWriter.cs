using Crane.Core.Api.Model;

namespace Crane.Core.Api.Writers
{
    public interface ISolutionWriter
    {
        void PatchAssemblyInfo(Project project);
    }
}