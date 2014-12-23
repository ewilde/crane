﻿using System;
using System.IO;
using Crane.Core.Commands.Exceptions;
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

        public InitCommandHandler(ICraneContext context, ITemplateResolver templateResolver, IFileManager fileManager, IProjectContextFactory projectContextFactory, ITemplateInvoker templateInvoker)
        {
            _context = context;
            _templateResolver = templateResolver;
            _fileManager = fileManager;
            _projectContextFactory = projectContextFactory;
            _templateInvoker = templateInvoker;
        }

        protected override void DoHandle(Init command)
        {
            
            CreateProject(command.ProjectName);
            CreateBuild(command.ProjectName);
        }

        private void CreateProject(string projectName)
        {
            var projectContext = _projectContextFactory.Create(projectName, projectName);
            _context.ProjectRootDirectory = new DirectoryInfo(Path.Combine(_fileManager.CurrentDirectory, projectContext.ProjectName));
            if (_fileManager.DirectoryExists(_context.ProjectRootDirectory.FullName))
                throw new DirectoryExistsCraneException(projectContext.ProjectName);

            _fileManager.CreateDirectory(_context.ProjectRootDirectory.FullName);

            var visualStudio = _templateResolver.Resolve(TemplateType.Source);
            if (visualStudio == null)
            {
                throw new Exception("Project template not found, please check your configuration");
            }

            _templateInvoker.InvokeTemplate(visualStudio, projectContext);            
        }

        private void CreateBuild(string projectName)
        {
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