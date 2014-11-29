using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;

namespace Crane.Core.Templates.Psake
{
    public class PsakeBuildTemplate : BuildTemplate
    {
        private readonly IFileManager _fileManager;
        private readonly ICraneContext _context;
        private readonly IConfiguration _configuration;
        private DirectoryInfo _templateSourceDirectory;

        public PsakeBuildTemplate(ICraneContext context, IConfiguration configuration, IFileManager fileManager)
        {
            _fileManager = fileManager;
            _context = context;
            _configuration = configuration;
        }


        protected override IEnumerable<FileInfo> TemplatedFiles
        {
            get { return new[] {this.BuildScript};}
        }

        protected override void CreateCore()
        {
            var destination = _context.BuildDirectory.FullName;
            if (!_fileManager.DirectoryExists(destination))
            {
                _fileManager.CreateDirectory(destination);
            }

            _fileManager.CopyFiles(TemplateSourceDirectory.FullName, destination, "*.*");

            var buildScript = _fileManager.ReadAllText(BuildScript.FullName).Replace("%context.Configuration.ProjectName%", _context.Configuration.ProjectName);
            _fileManager.WriteAllText(BuildScript.FullName, buildScript);
        }

        public override FileInfo BuildScript
        {
            get { return new FileInfo(Path.Combine(_context.BuildDirectory.FullName, "default.ps1")); }
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
                    _templateSourceDirectory = new DirectoryInfo(Path.Combine(_context.CraneInstallDirectory.FullName, "Templates", "Psake", "Files"));
                }

                return _templateSourceDirectory;
            }
            set { _templateSourceDirectory = value; }
        }
    }
}