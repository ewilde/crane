using System.IO;

namespace Crane.Core.Templates
{
    public interface ITemplate
    {
        string Name { get; }
        DirectoryInfo TemplateSourceDirectory { get; set; }
        void Create();
    }
}