using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Templates.VisualStudio
{
    public class VisualStudioTemplate : BaseTemplate
    {
        public VisualStudioTemplate(ICraneContext context, IConfiguration configuration, IFileManager fileManager, ITemplateParser templateParser, IFileAndDirectoryTokenParser fileAndDirectoryTokenParser) :
            base(context, configuration, fileManager, templateParser, fileAndDirectoryTokenParser)
        {
        }

        protected override void CreateCore()
        {
            var srcDir = Context.SourceDirectory.FullName;
            FileManager.CreateDirectory(srcDir);
            FileManager.CopyFiles(TemplateSourceDirectory.FullName, srcDir, "*.*");
            FileManager.RenameDirectory(Path.Combine(srcDir, "ClassLibrary1"), Context.ProjectName);
            FileManager.RenameDirectory(Path.Combine(srcDir, "ClassLibrary1.Tests"), Context.ProjectName);
        }

        public override string Name
        {
            get { return "VisualStudio"; }
        }

        protected override IEnumerable<FileInfo> TemplatedFiles
        {
            get
            {
                return new[]
                {
                    new FileInfo(Path.Combine(Context.SourceDirectory.FullName, string.Format("{0}.sln", Context.ProjectName)))
                };
            }
        }
    }
}