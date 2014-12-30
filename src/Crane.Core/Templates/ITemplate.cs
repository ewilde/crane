using System.IO;

namespace Crane.Core.Templates
{
    public interface ITemplate
    {
        string Name { get; set; }

        string InstallFolderRootName { get; set; }

        TemplateType TemplateType { get; set; }

        DirectoryInfo TemplateSourceDirectory { get; set; }        
    }
}