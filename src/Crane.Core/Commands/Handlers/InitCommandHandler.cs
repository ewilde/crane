using System;
using Crane.Core.Commands.Execution;
using Crane.Core.Configuration;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;

namespace Crane.Core.Commands.Handlers
{
    public class InitCommandHandler : CommandHandler<Init>
    {
        private readonly ITemplateResolver _templateResolver;
        private readonly IProjectContextFactory _projectContextFactory;
        private readonly ITemplateInvoker _templateInvoker;
        private readonly AssembleCommandHandler _assembleCommandHandler;
        

        public InitCommandHandler(ITemplateResolver templateResolver, 
            IProjectContextFactory projectContextFactory, 
            ITemplateInvoker templateInvoker,
            AssembleCommandHandler assembleCommandHandler)
        {
            _templateResolver = templateResolver;
            _projectContextFactory = projectContextFactory;
            _templateInvoker = templateInvoker;
            _assembleCommandHandler = assembleCommandHandler;
        }

        protected override void DoHandle(Init command)
        {            
            CreateProject(command.ProjectName);
            _assembleCommandHandler.Handle(new Assemble {FolderName = command.ProjectName});
        }

        private void CreateProject(string projectName)
        {
            var projectContext = _projectContextFactory.Create(projectName, projectName);
            var visualStudio = _templateResolver.Resolve(TemplateType.Source);
            _templateInvoker.InvokeTemplate(visualStudio, projectContext);            
        }
        
    }
}