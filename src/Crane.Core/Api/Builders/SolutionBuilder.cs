using System;
using System.Collections.Generic;
using System.IO;
using Crane.Core.Api.Model;
using Crane.Core.IO;

namespace Crane.Core.Api.Builders
{
    public class SolutionBuilder : ISolutionBuilder
    {
        private readonly ISolutionContext _solutionContext;
        private readonly ISolutionFactory _solutionFactory;
        private readonly IFileFactory _fileFactory;
        private readonly List<Project> _projects;
        private readonly List<PlainFile> _plainFiles;
        private Project _currentProject;
        private readonly Solution _solution;

        public SolutionBuilder(ISolutionContext solutionContext, ISolutionFactory solutionFactory, IFileFactory fileFactory)
        {
            _solutionContext = solutionContext;
            _solutionFactory = solutionFactory;
            _fileFactory = fileFactory;
            _projects = new List<Project>();
            _plainFiles = new List<PlainFile>();
            _solution = new Solution();
        }
        
        public ISolutionBuilder WithProject(Action<Project> assign)
        {
            var p = new Project
            {
                Solution = _solution
            };

            assign(p);
            _projects.Add(p);
            _currentProject = p;
            return this;
        }

        public ISolutionBuilder WithFile<T>(Action<T> assign) where T : ProjectFile, new()
        {
            if (_currentProject == null)
                throw new InvalidOperationException(
                    "Please add a project first using WithProject before attempting to add a file");

            var file = new T();
            assign(file);
            _currentProject.Files.Add(file);

            return this;
        }

        public ISolutionBuilder WithFile(Action<PlainFile> assign)
        {
            var file = new PlainFile();
            assign(file);
            _plainFiles.Add(file);

            return this;
        }

        public ISolutionBuilder WithSolution(Action<Solution> assign)
        {
            assign(_solution);
            _solutionContext.Path = new FileInfo(_solution.Path).DirectoryName;            
            return this;
        }

        public ISolutionContext Build()
        {
            _plainFiles.ForEach(_fileFactory.Create);

            var result = _solutionContext;
            result.Solution = _solutionFactory.Create(_solution.Path, _projects);
            result.Solution.SolutionContext = result;
            return result;
        }
    }
}