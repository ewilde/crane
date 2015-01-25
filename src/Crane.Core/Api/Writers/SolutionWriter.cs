using System.Dynamic;
using Crane.Core.Api.Model;

namespace Crane.Core.Api.Writers
{
    public class SolutionWriter : ISolutionWriter
    {
        private readonly IAssemblyInfoWriter _assemblyInfoWriter;

        public SolutionWriter(IAssemblyInfoWriter assemblyInfoWriter)
        {
            _assemblyInfoWriter = assemblyInfoWriter;
        }

        public void PatchAssemblyInfo(Project project)
        {
            _assemblyInfoWriter.Patch(project.AssemblyInfo);
        }
    }
}