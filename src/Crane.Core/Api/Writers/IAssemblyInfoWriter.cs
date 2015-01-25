using Crane.Core.Api.Model;

namespace Crane.Core.Api.Writers
{
    public interface IAssemblyInfoWriter
    {
        void Patch(AssemblyInfo assemblyInfo);

        void Create(string path, string title, string description, string version, string informationalVersion, string fileVersion);
    }
}