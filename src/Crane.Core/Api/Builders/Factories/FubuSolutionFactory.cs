using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Extensions;
using FubuCsProjFile;

namespace Crane.Core.Api.Builders
{
    public class FubuSolutionFactory : ISolutionFactory
    {
        public Solution Create(string fullName, IEnumerable<Project> projects)
        {
            var file = new FileInfo(fullName);
            var fubuProjects = projects.Select(project => 
                 project.Path == null ?
                FubuCsProjFile.CsProjFile.CreateAtSolutionDirectory(project.Name, file.DirectoryName) :
                FubuCsProjFile.CsProjFile.CreateAtLocation(project.Path, project.Name));            
            var fubuSolution = FubuCsProjFile.Solution.CreateNew(file.DirectoryName, file.Name);

            var csProjFiles = fubuProjects as CsProjFile[] ?? fubuProjects.ToArray();
            csProjFiles.ForEach(fubuSolution.AddProject);
            fubuSolution.Version = FubuCsProjFile.Solution.VS2013;
            fubuSolution.Save(saveProjects: true);

            return new Solution
            {
                Name = fubuSolution.Name,
                Path = fubuSolution.Filename,
                Projects = csProjFiles.Select(project => new Project {Name = project.ProjectName})
            };
        }
    }
}