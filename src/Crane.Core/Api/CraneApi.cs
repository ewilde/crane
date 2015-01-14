using System;
using System.Collections.Generic;

namespace Crane.Core.Api
{
    public static class CraneApi
    {
        public static SolutionContext GetSolutionContext(string rootFolderPath)
        {
            return new SolutionContext();
        }

        public static SolutionBuilder CreateSolution(string rootFolderPath)
        {
            return new SolutionBuilder(rootFolderPath);
        }
        
    }

    public class SolutionContext
    {
        public IEnumerable<Project> Projects { get; set; }
    }

    public class SolutionBuilder
    {
        private string _rootPath;        
        private List<Project> _projects; 

        public SolutionBuilder(string rootPath)
        {
            _rootPath = rootPath;            
            _projects = new List<Project>();
        }


        public SolutionBuilder WithProject(Action<Project> assign)
        {
            var p = new Project();
            assign(p);
            _projects.Add(p);
            return this;
        }

        public SolutionContext Build()
        {
            return null;
        }
    }

    public class ProjectBuilder
    {
        private Project _project;

        public ProjectBuilder()
        {
            _project = new Project();
        }


        public ProjectBuilder WithName(string name)
        {
            _project.Name = name;
            return this;
        }

        public Project Build()
        {
            return null;
        }
    }

    public class Project
    {
        public string Name { get;  set; }    
    }

    public class Test
    {
        public void Foo()
        {
            var solutionContext =
                CraneApi.CreateSolution("path")
                    .WithProject(p => p.Name = "SallyFx")
                    .WithProject(p => p.Name = "SallyFx.Common").Build();

            
        }
    }
}