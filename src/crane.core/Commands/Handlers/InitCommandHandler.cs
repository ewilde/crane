using System;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;

namespace Crane.Core.Commands.Handlers
{
    public class InitCommandHandler : CommandHandler<InitCommand>
    {
        private readonly ICraneContext _context;
        private readonly ITemplateResolver _templateResolver;
        private readonly IFileManager _fileManager;

        public InitCommandHandler(ICraneContext context, ITemplateResolver templateResolver, IFileManager fileManager)
        {
            _context = context;
            _templateResolver = templateResolver;
            _fileManager = fileManager;
        }

        protected override void DoHandle(InitCommand command)
        {
            _context.ProjectName = command.ProjectName;
            CreateProject();
            CreateBuild();
        }

        private void CreateProject()
        {
            _context.ProjectRootDirectory = new DirectoryInfo(Path.Combine(_fileManager.CurrentDirectory, _context.ProjectName));
            if (_fileManager.DirectoryExists(_context.ProjectRootDirectory.FullName))
                throw new CraneException(string.Format("directory {0} already exists", _context.ProjectName));

            _fileManager.CreateDirectory(_context.ProjectRootDirectory.FullName);

            var visualStudio = _templateResolver.Resolve(TemplateType.Source);
            if (visualStudio == null)
            {
                throw new Exception("Project template not found, please check your configuration");
            }

            visualStudio.Create();
        }

        private void CreateBuild()
        {
            var build = _templateResolver.Resolve(TemplateType.Build);
            if (build == null)
            {
                throw new Exception("Build template not found, please check your configuration");
            }

            build.Create();
        }
    }
}