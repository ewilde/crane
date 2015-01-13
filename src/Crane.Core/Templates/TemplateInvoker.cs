using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Parsers;

namespace Crane.Core.Templates
{
    public class TemplateInvoker : ITemplateInvoker
    {
        private readonly IFileManager _fileManager;
        private readonly ITemplateParser _templateParser;
        private readonly IFileAndDirectoryTokenParser _fileAndDirectoryTokenParser;
        private readonly ICraneContext _context;
        private readonly ITokenDictionaryFactory _tokenDictionaryFactory;

        public TemplateInvoker(IFileManager fileManager, 
            ITemplateParser templateParser, 
            IFileAndDirectoryTokenParser fileAndDirectoryTokenParser, 
            ICraneContext context, 
            ITokenDictionaryFactory tokenDictionaryFactory)
        {
            _fileManager = fileManager;
            _templateParser = templateParser;
            _fileAndDirectoryTokenParser = fileAndDirectoryTokenParser;
            _context = context;
            _tokenDictionaryFactory = tokenDictionaryFactory;
        }

        
        private IEnumerable<FileInfo> GetInstalledFiles(ITemplate template)
        {           
            var templateInstallDirectory = new DirectoryInfo(Path.Combine(_context.ProjectRootDirectory.FullName, template.InstallFolderRootName));
            return _fileManager
                .EnumerateFiles(templateInstallDirectory.FullName, "*.*", SearchOption.AllDirectories)
                .Select(item => new FileInfo(item));
            
        }
       
        private void ParseTemplate(ITemplate template, ITokenDictionary tokenDictionary)
        {
            foreach (var file in GetInstalledFiles(template).Where(f => f.IsTextFile()))
            {
                var parsed = _templateParser.Parse(tokenDictionary, _fileManager.ReadAllText(file.FullName));
                _fileManager.WriteAllText(file.FullName, parsed);
            }
        }


        public void InvokeTemplate(ITemplate template, IProjectContext projectContext)
        {
            _context.ProjectRootDirectory =
                    new DirectoryInfo(Path.Combine(_fileManager.CurrentDirectory, projectContext.ProjectName));

            if (template.TemplateType == TemplateType.Source)
            {
                
                if (_fileManager.DirectoryExists(_context.ProjectRootDirectory.FullName))
                    throw new DirectoryExistsCraneException(projectContext.ProjectName);

                _fileManager.CreateDirectory(_context.ProjectRootDirectory.FullName);
            }

           
            var tokenDictionary = _tokenDictionaryFactory.Create(_context, projectContext);
            _fileManager.CopyFiles(template.TemplateSourceDirectory.FullName, _context.ProjectRootDirectory.FullName, true);
            _fileAndDirectoryTokenParser.Parse(_context.ProjectRootDirectory, tokenDictionary);
            ParseTemplate(template, tokenDictionary);
        }


    }
}