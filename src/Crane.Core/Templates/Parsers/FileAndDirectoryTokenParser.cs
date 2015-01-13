using System.IO;
using Crane.Core.Extensions;
using Crane.Core.IO;

namespace Crane.Core.Templates.Parsers
{
    public class FileAndDirectoryTokenParser : IFileAndDirectoryTokenParser
    {
        private readonly IFileManager _fileManager;        

        public FileAndDirectoryTokenParser(
            IFileManager fileManager)
        {
            _fileManager = fileManager;            
        }

        public void Parse(DirectoryInfo directory, ITokenDictionary tokenDictionary)
        {
            if (directory.Exists)
            {
                directory.EnumerateFiles("*.*", SearchOption.AllDirectories).ForEach(p => ParseFile(p, tokenDictionary));
            }

            ParseDirectory(directory, tokenDictionary);

            if (directory.Exists)
            {
                directory.EnumerateDirectories("*.*", SearchOption.AllDirectories).ForEach(p => ParseDirectory(p, tokenDictionary));
            }
        }

        protected void ParseDirectory(DirectoryInfo directory, ITokenDictionary tokenDictionary)
        {
            foreach (var token in tokenDictionary.Tokens)
            {
                if (directory.Name.Contains(token.Key))
                {
                    _fileManager.RenameDirectory(directory.FullName, directory.Name.Replace(token.Key, token.Value.Invoke()));
                }
            }
        }

        protected void ParseFile(FileInfo path, ITokenDictionary tokenDictionary)
        {
            foreach (var token in tokenDictionary.Tokens)
            {
                if (path.Name.Contains(token.Key))
                {
                    _fileManager.RenameFile(path.FullName, path.Name.Replace(token.Key, token.Value.Invoke()));
                }
            }
        }
    }
}
