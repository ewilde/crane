using System;
using Crane.Core.Commands.Resolvers;
using Crane.Core.Configuration;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;

namespace Crane.Core.Commands.Handlers
{
    public class AssembleCommandHandler : CommandHandler<Assemble>
    {
        private readonly IProjectContextFactory _projectContextFactory;
        private readonly ITemplateResolver _templateResolver;
        private readonly ITemplateInvoker _templateInvoker;
        private readonly ISolutionPathResolver _solutionPathResolver;

        public AssembleCommandHandler(IProjectContextFactory projectContextFactory, ITemplateResolver templateResolver, ITemplateInvoker templateInvoker, ISolutionPathResolver solutionPathResolver)
        {
            _projectContextFactory = projectContextFactory;
            _templateResolver = templateResolver;
            _templateInvoker = templateInvoker;
            _solutionPathResolver = solutionPathResolver;
        }

        protected override void DoHandle(Assemble command)
        {
            var projectName = command.FolderName;
            var solutionPath = _solutionPathResolver.GetPathRelativeFromBuildFolder(command.FolderName);
            var projectContext = _projectContextFactory.Create(projectName, solutionPath);
            var buildTemplate = _templateResolver.Resolve(TemplateType.Build);

            if (buildTemplate == null)
            {
                throw new Exception("Build template not found, please check your configuration");
            }

            _templateInvoker.InvokeTemplate(buildTemplate, projectContext);
        }
    }
}