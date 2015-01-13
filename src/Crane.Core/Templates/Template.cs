using System.IO;

namespace Crane.Core.Templates
{
    public class Template : ITemplate
    {
        
        
           
        public string Name { get; set; }

        public TemplateType TemplateType { get; set; }

        public DirectoryInfo TemplateSourceDirectory { get; set; }

        public string InstallFolderRootName { get; set; }
        
       
        
    }
}