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
        private readonly IRelativeSolutionPathToBuildFolderResolver _solutionPathToBuildFolderResolver;

        public AssembleCommandHandler(IProjectContextFactory projectContextFactory, ITemplateResolver templateResolver, ITemplateInvoker templateInvoker, IRelativeSolutionPathToBuildFolderResolver solutionPathToBuildFolderResolver)
        {
            _projectContextFactory = projectContextFactory;
            _templateResolver = templateResolver;
            _templateInvoker = templateInvoker;
            _solutionPathToBuildFolderResolver = solutionPathToBuildFolderResolver;
        }

        protected override void DoHandle(Assemble command)
        {
            var projectName = command.FolderName;
            var solutionPath = _solutionPathToBuildFolderResolver.ResolveSolutionPath(command.FolderName);
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