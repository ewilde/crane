using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Templates.Psake
{
    public class PsakeBuildTemplate : BuildTemplate
    {
        private DirectoryInfo _templateSourceDirectory;

        public PsakeBuildTemplate(ICraneContext context, IConfiguration configuration, IFileManager fileManager, ITemplateParser templateParser) :
            base(context, configuration, fileManager, templateParser)
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

        public override FileInfo BuildScript
        {
            get { return new FileInfo(Path.Combine(Context.BuildDirectory.FullName, "default.ps1")); }
        }

        public override string Name
        {
            get { return "Psake"; }
        }

        public override DirectoryInfo TemplateSourceDirectory
        {
            get
            {
                if (_templateSourceDirectory == null)
                {
                    _templateSourceDirectory = new DirectoryInfo(Path.Combine(Context.CraneInstallDirectory.FullName, "Templates", "Psake", "Files"));
                }

                return _templateSourceDirectory;
            }
            set { _templateSourceDirectory = value; }
        }
    }
}