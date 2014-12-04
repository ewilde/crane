using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Templates.Psake
{
    public class PsakeBuildTemplate : BaseTemplate, IBuildTemplate
    {
        public PsakeBuildTemplate(ICraneContext context, IConfiguration configuration, IFileManager fileManager, ITemplateParser templateParser, IFileAndDirectoryTokenParser fileAndDirectoryTokenParser) :
            base(context, configuration, fileManager, templateParser, fileAndDirectoryTokenParser)
        {
        }

        protected override IEnumerable<FileInfo> TemplatedFiles
        {
            get { return new[] {this.BuildScript};}
        }

        protected override void CreateCore()
        {
            FileManager.CopyFiles(Path.Combine(TemplateSourceDirectory.FullName), Context.ProjectRootDirectory.FullName, true);
        }

        public FileInfo BuildScript
        {
            get { return new FileInfo(Path.Combine(Context.BuildDirectory.FullName, "default.ps1")); }
        }

        public override string Name
        {
            get { return "Psake"; }
        }

        public override TemplateType TemplateType
        {
            get { return TemplateType.Build; }
        }
    }
}