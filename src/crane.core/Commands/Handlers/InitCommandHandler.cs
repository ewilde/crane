using System;
using System.IO;
using Crane.Core.Commands.Exceptions;
using Crane.Core.Commands.Execution;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;

namespace Crane.Core.Commands.Handlers
{
    public class InitCommandHandler : CommandHandler<Init>
    {
        private readonly ICraneContext _context;
        private readonly ITemplateResolver _templateResolver;
        private readonly IFileManager _fileManager;
        private readonly IProjectContextFactory _projectContextFactory;
        private readonly ITemplateInvoker _templateInvoker;
        private readonly Func<ICommandExecutor> _commandExecutorFactory; 
        

        public InitCommandHandler(ICraneContext context, 
            ITemplateResolver templateResolver, 
            IFileManager fileManager, 
            IProjectContextFactory projectContextFactory, 
            ITemplateInvoker templateInvoker,
            Func<ICommandExecutor> commandExecutorFactory)
        {
            _context = context;
            _templateResolver = templateResolver;
            _fileManager = fileManager;
            _projectContextFactory = projectContextFactory;
            _templateInvoker = templateInvoker;
            _commandExecutorFactory = commandExecutorFactory;
        }

        protected override void DoHandle(Init command)
        {
            
            CreateProject(command.ProjectName);
            var executor = _commandExecutorFactory();
            executor.ExecuteCommand("Assemble", "-folderName", command.ProjectName);            
        }

        private void CreateProject(string projectName)
        {
            var projectContext = _projectContextFactory.Create(projectName, projectName);
            
            var visualStudio = _templateResolver.Resolve(TemplateType.Source);
            if (visualStudio == null)
            {
                throw new TemplateNotFoundCraneException(TemplateType.Source);
            }

            _templateInvoker.InvokeTemplate(visualStudio, projectContext);            
        }
        
    }
}