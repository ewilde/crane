using System.Collections.Generic;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Templates
{
    public abstract class BaseTemplate : ITemplate
    {
        private readonly IFileManager _fileManager;
        private readonly ITemplateParser _templateParser;
        private readonly IFileAndDirectoryTokenParser _fileAndDirectoryTokenParser;
        private readonly ICraneContext _context;
        private readonly IConfiguration _configuration;
        private DirectoryInfo _templateSourceDirectory;

        protected BaseTemplate(ICraneContext context, IConfiguration configuration, IFileManager fileManager, ITemplateParser templateParser, IFileAndDirectoryTokenParser fileAndDirectoryTokenParser)
        {
            _fileManager = fileManager;
            _templateParser = templateParser;
            _fileAndDirectoryTokenParser = fileAndDirectoryTokenParser;
            _context = context;
            _configuration = configuration;
        }
            
        protected abstract void CreateCore();
        
        public abstract string Name { get; }

        public DirectoryInfo TemplateSourceDirectory
        {
            get
            {
                if (_templateSourceDirectory == null)
                {
                    _templateSourceDirectory = new DirectoryInfo(Path.Combine(Context.CraneInstallDirectory.FullName, "Templates", this.Name, "Files"));
                }

                return _templateSourceDirectory;
            }
            set { _templateSourceDirectory = value; }
        }
        
        protected abstract IEnumerable<FileInfo> TemplatedFiles { get; }

        protected IFileManager FileManager
        {
            get { return _fileManager; }
        }

        protected ICraneContext Context
        {
            get { return _context; }
        }

        protected IConfiguration Configuration
        {
            get { return _configuration; }
        }

        public void Create()
        {
            this.CreateCore();
            this.ParseTemplate();
            this.ParseDirectories();
        }

        private void ParseDirectories()
        {
            _fileAndDirectoryTokenParser.Parse(_context.ProjectRootDirectory.FullName);
        }

        protected virtual void ParseTemplate()
        {
            foreach (var file in this.TemplatedFiles)
            {
                var parsed = _templateParser.Parse(FileManager.ReadAllText(file.FullName), this.Context);
                FileManager.WriteAllText(file.FullName, parsed);
            }
        }
    }
}