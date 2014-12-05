using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Templates.VisualStudio
{
    public class VisualStudioTemplate : BaseTemplate
    {
        public VisualStudioTemplate(
            ICraneContext context, 
            IConfiguration configuration, 
            IFileManager fileManager, 
            ITemplateParser templateParser, 
            IFileAndDirectoryTokenParser fileAndDirectoryTokenParser) :
            base(context, configuration, fileManager, templateParser, fileAndDirectoryTokenParser)
        {
        }

        protected override void CreateCore()
        {
            var srcDir = Context.ProjectRootDirectory.FullName;
            FileManager.CreateDirectory(srcDir);
            FileManager.CopyFiles(Path.Combine(TemplateSourceDirectory.FullName, "2013"), srcDir, true);
        }

        public override string Name
        {
            get { return "VisualStudio"; }
        }

        public override TemplateType TemplateType
        {
            get { return  TemplateType.Source; }
        }

        protected override IEnumerable<FileInfo> TemplatedFiles
        {
            get
            {
                if (TemplateTargetDirectory.Exists)
                {
                    return TemplateTargetDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories);
                }

                return Enumerable.Empty<FileInfo>();
            }
        }
    }
}