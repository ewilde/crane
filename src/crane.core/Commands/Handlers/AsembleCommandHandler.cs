using System;
using Crane.Core.Configuration;
using Crane.Core.Templates;
using Crane.Core.Templates.Resolvers;

namespace Crane.Core.Commands.Handlers
{
    public class AsembleCommandHandler : CommandHandler<Assemble>
    {
        private readonly IProjectContextFactory _projectContextFactory;
        private readonly ITemplateResolver _templateResolver;
        private readonly ITemplateInvoker _templateInvoker;

        public AsembleCommandHandler(IProjectContextFactory projectContextFactory, ITemplateResolver templateResolver, ITemplateInvoker templateInvoker)
        {
            _projectContextFactory = projectContextFactory;
            _templateResolver = templateResolver;
            _templateInvoker = templateInvoker;
        }

        protected override void DoHandle(Assemble command)
        {
            var projectName = command.FolderName;
            var projectContext = _projectContextFactory.Create(projectName, projectName);
            var buildTemplate = _templateResolver.Resolve(TemplateType.Build);
            if (buildTemplate == null)
            {
                throw new Exception("Build template not found, please check your configuration");
            }

            _templateInvoker.InvokeTemplate(buildTemplate, projectContext);
        }
    }
}