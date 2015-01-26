using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Api.Model;
using Crane.Core.Api.Model.Mappers;
using Crane.Core.Api.Writers;
using Crane.Core.Extensions;
using FubuCsProjFile;
using Solution = Crane.Core.Api.Model.Solution;

namespace Crane.Core.Api.Builders
{
    [CLSCompliant(false)]
    public class FubuSolutionFactory : ISolutionFactory
    {
        private readonly IFubuSolutionMapper _mapper;
        private readonly IAssemblyInfoWriter _assemblyInfoWriter;
        
        public FubuSolutionFactory(IFubuSolutionMapper mapper, IAssemblyInfoWriter assemblyInfoWriter)
        {
            _mapper = mapper;
            _assemblyInfoWriter = assemblyInfoWriter;
        }

        public Solution Create(string fullName, IEnumerable<Project> projects)
        {
            var file = new FileInfo(fullName);
            var fubuProjects = projects.Select(project => CreateProject(project, file));            
            var fubuSolution = FubuCsProjFile.Solution.CreateNew(file.DirectoryName, file.Name);

            var csProjFiles = fubuProjects as CsProjFile[] ?? fubuProjects.ToArray();
            csProjFiles.ForEach(fubuSolution.AddProject);
            fubuSolution.Version = FubuCsProjFile.Solution.VS2013;
            fubuSolution.Save(saveProjects: true);

            return _mapper.Map(fubuSolution);
        }

        private CsProjFile CreateProject(Project project, FileInfo file)
        {
            var projFile = project.Path == null ?
                FubuCsProjFile.CsProjFile.CreateAtSolutionDirectory(project.Name, file.DirectoryName) :
                FubuCsProjFile.CsProjFile.CreateAtLocation(project.Path, project.Name);

            if (project.AssemblyInfo != null && projFile.AssemblyInfo == null)
            {
                var assemblyInfo = project.AssemblyInfo;
                var assemblyInfoPath = Path.Combine(projFile.ProjectDirectory, "Properties", "AssemblyInfo.cs");
                _assemblyInfoWriter.Create(assemblyInfoPath,
                    assemblyInfo.Title, assemblyInfo.Description, assemblyInfo.Version.ToString(),
                    assemblyInfo.InformationalVersion, assemblyInfo.FileVersion.ToString());

                projFile.Add<CodeFile>(Path.Combine("Properties", "AssemblyInfo.cs"));
            }

            return projFile;
        }
    }
}