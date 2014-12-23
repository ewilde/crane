using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Templates
{
    public interface ITemplateInvoker
    {
        void InvokeTemplate(ITemplate template, IProjectContext projectContext);
    }

    public class TemplateInvoker : ITemplateInvoker
    {
        private readonly IFileManager _fileManager;
        private readonly ITemplateParser _templateParser;
        private readonly IFileAndDirectoryTokenParser _fileAndDirectoryTokenParser;
        private readonly ICraneContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenDictionaryFactory _tokenDictionaryFactory;

        public TemplateInvoker(IFileManager fileManager, ITemplateParser templateParser, IFileAndDirectoryTokenParser fileAndDirectoryTokenParser, ICraneContext context, IConfiguration configuration, ITokenDictionaryFactory tokenDictionaryFactory)
        {
            _fileManager = fileManager;
            _templateParser = templateParser;
            _fileAndDirectoryTokenParser = fileAndDirectoryTokenParser;
            _context = context;
            _configuration = configuration;
            _tokenDictionaryFactory = tokenDictionaryFactory;
        }

        private string GetTemplateInstallRootFolderName(TemplateType templateType)
        {

            switch (templateType)
            {
                case TemplateType.Build:
                    return _configuration.BuildFolderName;
                case TemplateType.Source:
                    return _configuration.SourceFolderName;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
        private IEnumerable<FileInfo> GetInstalledFiles(ITemplate template)
        {           
            var templateInstallDirectory =
                new DirectoryInfo(Path.Combine(_context.ProjectRootDirectory.FullName,
                    GetTemplateInstallRootFolderName(template.TemplateType)));
            return _fileManager
                .EnumerateFiles(templateInstallDirectory.FullName, "*.*",
                    SearchOption.AllDirectories)
                .Select(item => new FileInfo(item));
            
        }

       

        private void ParseTemplate(ITemplate template, ITokenDictionary tokenDictionary)
        {
            foreach (var file in GetInstalledFiles(template).Where(IsTextFile))
            {
                var parsed = _templateParser.Parse(tokenDictionary, _fileManager.ReadAllText(file.FullName));
                _fileManager.WriteAllText(file.FullName, parsed);
            }
        }

        private bool IsTextFile(FileInfo file)
        {
            return !file.Extension.Equals(".exe");
        }

        public void InvokeTemplate(ITemplate template, IProjectContext projectContext)
        {
            var tokenDictionary = _tokenDictionaryFactory.Create(_context, projectContext);
            _fileManager.CopyFiles(template.TemplateSourceDirectory.FullName, _context.ProjectRootDirectory.FullName, true);
            _fileAndDirectoryTokenParser.Parse(_context.ProjectRootDirectory, tokenDictionary);
            ParseTemplate(template, tokenDictionary);
        }


    }
    public class Template : ITemplate
    {
        
        
           
        public string Name { get; set; }

        public TemplateType TemplateType { get; set; }

        public DirectoryInfo TemplateSourceDirectory { get; set; }
        
       
        
    }
}