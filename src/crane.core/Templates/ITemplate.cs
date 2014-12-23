using System.IO;
using Crane.Core.Configuration;

namespace Crane.Core.Templates
{
    public interface ITemplate
    {
        string Name { get; set; }

        TemplateType TemplateType { get; set; }

        DirectoryInfo TemplateSourceDirectory { get; set; }
        void Create(IProjectContext projectContext);
    }
}