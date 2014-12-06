using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;
using Crane.Core.Utility;

namespace Crane.Core.Templates
{
    public class BaseTemplate : ITemplate
    {
        private readonly IFileManager _fileManager;
        private readonly ITemplateParser _templateParser;
        private readonly IFileAndDirectoryTokenParser _fileAndDirectoryTokenParser;
        private readonly ICraneContext _context;
        private readonly IConfiguration _configuration;
        private DirectoryInfo _templateInstallDirectory;

        public BaseTemplate(
            ICraneContext context, 
            IConfiguration configuration, 
            IFileManager fileManager, 
            ITemplateParser templateParser, 
            IFileAndDirectoryTokenParser fileAndDirectoryTokenParser)
        {
            _fileManager = fileManager;
            _templateParser = templateParser;
            _fileAndDirectoryTokenParser = fileAndDirectoryTokenParser;
            _context = context;
            _configuration = configuration;
        }
           
        public string Name { get; set; }

        public TemplateType TemplateType { get; set; }

        public DirectoryInfo TemplateSourceDirectory { get; set; }

        public DirectoryInfo TemplateInstallDirectory
        {
            get
            {
                if (_templateInstallDirectory == null)
                {
                    _templateInstallDirectory = new DirectoryInfo(Path.Combine(this.Context.ProjectRootDirectory.FullName, this.TemplateInstallRootFolderName));
                }

                return _templateInstallDirectory; 
            }
        }

        public string TemplateInstallRootFolderName
        {
            get
            {
                switch (TemplateType)
                {
                    case TemplateType.Build:
                        return _configuration.BuildFolderName;
                    case TemplateType.Source:
                        return _configuration.SourceFolderName;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected IEnumerable<FileInfo> SourceFiles
        {
            get
            {
                return FileManager
                    .EnumerateFiles(this.TemplateSourceDirectory.FullName, "*.*", SearchOption.AllDirectories)
                    .Select(item => new FileInfo(item));
            }
        }

        protected IEnumerable<FileInfo> InstalledFiles
        {
            get
            {
                return FileManager
                    .EnumerateFiles(this.TemplateInstallDirectory.FullName, "*.*",
                        SearchOption.AllDirectories)
                    .Select(item => new FileInfo(item));
            }
        } 

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
            FileManager.CopyFiles(this.TemplateSourceDirectory.FullName, _context.ProjectRootDirectory.FullName, true);
            this.RenameDirectoriesAndFiles();
            this.ParseTemplate();
        }

        private void RenameDirectoriesAndFiles()
        {
            _fileAndDirectoryTokenParser.Parse(_context.ProjectRootDirectory);
        }

        protected virtual void ParseTemplate()
        {
            foreach (var file in this.InstalledFiles.Where(IsTextFile))
            {
                var parsed = _templateParser.Parse(FileManager.ReadAllText(file.FullName), this.Context);
                FileManager.WriteAllText(file.FullName, parsed);
            }
        }

        private bool IsTextFile(FileInfo file)
        {
            return !file.Extension.Equals(".exe");
        }
    }
}