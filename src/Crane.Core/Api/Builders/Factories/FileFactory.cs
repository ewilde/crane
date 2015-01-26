using System.IO;
using Crane.Core.IO;

namespace Crane.Core.Api.Builders
{
    public class FileFactory : IFileFactory
    {
        private readonly IFileManager _fileManager;

        public FileFactory(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public void Create(PlainFile file)
        {
            _fileManager.EnsureDirectoryExists(new FileInfo(file.Path).Directory);
            _fileManager.WriteAllText(file.Path, file.Text);
        }
    }
}