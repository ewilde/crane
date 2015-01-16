using System;
using System.Collections.Generic;

namespace Crane.Core.Api.Builders
{
    public class SolutionBuilder : ISolutionBuilder
    {
        private readonly ISolutionContext _solutionContext;
        private readonly ISolutionFactory _solutionFactory;
        private readonly List<Project> _projects; 

        public SolutionBuilder(ISolutionContext solutionContext, ISolutionFactory solutionFactory)
        {
            _solutionContext = solutionContext;
            _solutionFactory = solutionFactory;
            _projects = new List<Project>();
        }

        public string RootPath { get; set; }


        public ISolutionBuilder WithProject(Action<Project> assign)
        {
            var p = new Project();
            assign(p);
            _projects.Add(p);
            return this;
        }

        public ISolutionContext Build()
        {
            var result = _solutionContext;
            result.Solution = _solutionFactory.Create(RootPath, _projects);
            return result;
        }
    }
}