using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Utility;

namespace Crane.Core.Templates.Parsers
{
    public class FileAndDirectoryTokenParser : IFileAndDirectoryTokenParser
    {
        private readonly IFileManager _fileManager;
        private readonly ITokenDictionary _tokenDictionary;

        public FileAndDirectoryTokenParser(
            IFileManager fileManager, 
            ITokenDictionary tokenDictionary)
        {
            _fileManager = fileManager;
            _tokenDictionary = tokenDictionary;
        }

        public void Parse(DirectoryInfo directory)
        {
            if (directory.Exists)
            {
                directory.EnumerateFiles("*.*", SearchOption.AllDirectories).ForEach(ParseFile);
            }

            ParseDirectory(directory);

            if (directory.Exists)
            {
                directory.EnumerateDirectories("*.*", SearchOption.AllDirectories).ForEach(ParseDirectory);
            }
        }

        protected void ParseDirectory(DirectoryInfo directory)
        {
            foreach (var token in _tokenDictionary.Tokens)
            {
                if (directory.Name.Contains(token.Key))
                {
                    _fileManager.RenameDirectory(directory.FullName, directory.Name.Replace(token.Key, token.Value.Invoke()));
                }
            }
        }

        protected void ParseFile(FileInfo path)
        {
            foreach (var token in _tokenDictionary.Tokens)
            {
                if (path.FullName.Contains(token.Key))
                {
                    _fileManager.RenameFile(path.FullName, path.Name.Replace(token.Key, token.Value.Invoke()));
                }
            }
        }
    }
}
