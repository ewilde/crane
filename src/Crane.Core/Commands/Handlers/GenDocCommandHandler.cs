using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Utility;

namespace Crane.Core.Commands.Handlers
{
    public class GenDocCommandHandler : CommandHandler<GenDoc>
    {
        private readonly IFileManager _fileManager;

        public GenDocCommandHandler(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        protected override void DoHandle(GenDoc command)
        {
            var root = Path.GetFullPath(Path.Combine(this.GetType().Assembly.GetLocation().FullName, ".."));
            var doc = Path.Combine(root, "doc");
                
            if (!_fileManager.DirectoryExists(doc))
            {
                _fileManager.CreateDirectory(doc);
            }
        }
    }
}