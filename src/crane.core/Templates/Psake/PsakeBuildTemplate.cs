using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;

namespace Crane.Core.Templates.Psake
{
    public class PsakeBuildTemplate : IBuildTemplate
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


        public void Create()
        {
            var destination = _context.BuildDirectory.FullName;
            if (!_fileManager.DirectoryExists(destination))
            {
                _fileManager.CreateDirectory(destination);
            }

            _fileManager.CopyFiles(TemplateSourceDirectory.FullName,
                destination, "*.*");
        }

        public string BuildScript
        {
            get { return _fileManager.ReadAllText(Path.Combine(_context.BuildDirectory.FullName, "default.ps1")); }
        }

        public string Name
        {
            get { return "Psake"; }
        }

        public DirectoryInfo TemplateSourceDirectory
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