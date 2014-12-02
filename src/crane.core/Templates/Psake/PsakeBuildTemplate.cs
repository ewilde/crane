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
            var destination = Context.BuildDirectory.FullName;
            if (!FileManager.DirectoryExists(destination))
            {
                FileManager.CreateDirectory(destination);
            }

            FileManager.CopyFiles(TemplateSourceDirectory.FullName, destination, "*.*");

            var buildScript = FileManager.ReadAllText(BuildScript.FullName).Replace("%context.ProjectName%", Context.ProjectName);
            FileManager.WriteAllText(BuildScript.FullName, buildScript);
        }

        public FileInfo BuildScript
        {
            get { return new FileInfo(Path.Combine(Context.BuildDirectory.FullName, "default.ps1")); }
        }

        public override string Name
        {
            get { return "Psake"; }
        }
    }
}