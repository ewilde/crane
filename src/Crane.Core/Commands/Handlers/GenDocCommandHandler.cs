using System.Collections.Generic;
using System.IO;
using Crane.Core.Commands.Resolvers;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Utility;

namespace Crane.Core.Commands.Handlers
{
    public class GenDocCommandHandler : CommandHandler<GenDoc>
    {
        private readonly IFileManager _fileManager;
        private readonly IEnumerable<ICraneCommand> _commands;
        private string _docDirectory;

        public GenDocCommandHandler(IFileManager fileManager, IPublicCommandResolver commandResolver)
        {
            _fileManager = fileManager;
            _commands = commandResolver.Resolve();
        }

        protected override void DoHandle(GenDoc command)
        {
            var root = Path.GetFullPath(Path.Combine(this.GetType().Assembly.GetLocation().FullName, ".."));
            _docDirectory = Path.Combine(root, "doc");
                
            if (!_fileManager.DirectoryExists(_docDirectory))
            {
                _fileManager.CreateDirectory(_docDirectory);
            }

            this.CreateIndex();
            _commands.ForEach(this.CreateCommandPage);
        }

        private void CreateIndex()
        {
            _fileManager.WriteAllText(Path.Combine(_docDirectory, "index.md"), string.Empty);
        }

        private void CreateCommandPage(ICraneCommand command)
        {
            _fileManager.WriteAllText(Path.Combine(_docDirectory, command.Name() + ".md"), string.Empty);            
        }
    }
}