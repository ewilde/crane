using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Api.Model.Mappers;
using Crane.Core.Extensions;
using FubuCsProjFile;

namespace Crane.Core.Api.Builders
{
    [CLSCompliant(false)]
    public class FubuSolutionFactory : ISolutionFactory
    {
        private readonly IFubuSolutionMapper _mapper;

        public FubuSolutionFactory(IFubuSolutionMapper mapper)
        {
            _mapper = mapper;
        }

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

            return _mapper.Map(fubuSolution);
        }
    }
}