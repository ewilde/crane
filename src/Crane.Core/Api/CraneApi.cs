using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crane.Core.Api.Exceptions;
using Crane.Core.Api.Model;
using Crane.Core.Api.Readers;
using Crane.Core.Api.Writers;
using Crane.Core.Commands.Resolvers;
using Crane.Core.Extensions;
using Crane.Core.Runners;

namespace Crane.Core.Api
{
    public class CraneApi : ICraneApi
    {
        private readonly ISolutionReader _solutionReader;
        private readonly IAssemblyInfoWriter _assemblyInfoWriter;
        private readonly Func<ISolutionContext> _solutionContext;
        private readonly ISolutionPathResolver _solutionPathResolver;
        private readonly ISourceControlInformationReader _sourceControlInformationReader;
        private readonly INuGet _nuGet;
        private readonly IChocolatey _chocolatey;

        public CraneApi(
            ISolutionReader solutionReader,
            IAssemblyInfoWriter assemblyInfoWriter,
            Func<ISolutionContext> solutionContext, 
            ISolutionPathResolver solutionPathResolver, 
            ISourceControlInformationReader sourceControlInformationReader,
            INuGet nuGet,
            IChocolatey chocolatey)
        {
            _solutionReader = solutionReader;
            _assemblyInfoWriter = assemblyInfoWriter;
            _solutionContext = solutionContext;
            _solutionPathResolver = solutionPathResolver;
            _sourceControlInformationReader = sourceControlInformationReader;
            _nuGet = nuGet;
            _chocolatey = chocolatey;
        }

        public ISolutionContext GetSolutionContext(string rootFolderPath)
        {
            var context = _solutionContext();
            
            if (rootFolderPath.EndsWith(".sln"))
            {
                context.Solution = _solutionReader.FromPath(rootFolderPath);
                var directoryName = new FileInfo(rootFolderPath).DirectoryName;
                if (directoryName != null)
                {
                    var directoryInfo = new DirectoryInfo(directoryName).Parent;
                    if (directoryInfo != null)
                    {
                        context.Path = directoryInfo.FullName;
                    }
                    else
                    {
                        throw new SolutionContextException(string.Format("Could not create solution context. {0} does not have a parent directory.", directoryName));
                    }
                }
                else
                {
                    throw new SolutionContextException(string.Format("Could not create solution context. {0} does not have a valid directory.", rootFolderPath));
                }
            }
            else
            {
                context.Solution =
                    _solutionReader.FromPath(Path.Combine(rootFolderPath, _solutionPathResolver.GetPath(rootFolderPath)));
                context.Path = rootFolderPath;
            }
            context.Solution.SolutionContext = context;
            return context;
        }

        public void PatchAssemblyInfo(AssemblyInfo assemblyInfo)
        {
            _assemblyInfoWriter.Patch(assemblyInfo);            
        }

        public void PatchSolutionAssemblyInfo(ISolutionContext solutionContext, string version)
        {
            var sourceControlInformation = GetSourceControlInformation(solutionContext);


            foreach (var project in solutionContext.Solution.Projects.Where(p => !p.TestProject && p.AssemblyInfo != null))
            {
                var ver = new Version(version);
                project.AssemblyInfo.Version = ver;
                project.AssemblyInfo.FileVersion = ver;

                if (sourceControlInformation == null)
                {
                    project.AssemblyInfo.InformationalVersion = ver.ToString();
                }
                else
                {
                    project.AssemblyInfo.InformationalVersion = string.Format("{0} / {1}", ver,
                        sourceControlInformation.LastCommitMessage);
                }


                _assemblyInfoWriter.Patch(project.AssemblyInfo);
            }    
        }

        public ISourceControlInformation GetSourceControlInformation(ISolutionContext solutionContext)
        {
            return _sourceControlInformationReader.ReadSourceControlInformation(solutionContext);
        }

        public IEnumerable<RunResult> NugetPublish(ISolutionContext solutionContext, string nugetOutputPath, string version, string source, string apiKey)
        {
            var nugetProjects  = GetNugetProjects(solutionContext).ToArray();
            var results = new List<RunResult>(nugetProjects.Length);

            nugetProjects.ForEach(
                item =>
                {
                    var result = _nuGet.Publish(
                            Path.Combine(solutionContext.Path, "build", "NuGet.exe"),
                            Path.Combine(nugetOutputPath, 
                            string.Format("{0}.{1}.nupkg", item.Name, version)),
                            source, apiKey);
                    results.Add(result);

                    if (!_nuGet.ValidateResult(result))
                    {
                        throw new NuGetException(string.Format("Error executing nuget push for project {0}.{1}{2}",
                            item.Name, Environment.NewLine, result));
                    }
                });

            return results;
        }

        public IEnumerable<RunResult> NugetPack(ISolutionContext solutionContext, string buildOutputPath, string nugetOutputPath, string version)
        {
            var nugetProjects = GetNugetProjects(solutionContext).ToArray();

            var nugetExePath = Path.Combine(solutionContext.Path, "build", "NuGet.exe");
            if (!File.Exists(nugetExePath))
            {
                throw new FileNotFoundException(string.Format("Could not find file {0}.", nugetExePath), nugetExePath);
            }

            foreach (var item in nugetProjects)
            {
                var result = _nuGet.Pack(
                    nugetExePath, 
                    item.NugetSpec.Path, 
                    nugetOutputPath,
                    new List<Tuple<string, string>>
                    {
                        new Tuple<string, string>("version_number", version),
                        new Tuple<string, string>("build_output", buildOutputPath)
                    });
                   
                if (!_nuGet.ValidateResult(result))
                {
                    throw new NuGetException(string.Format("Error executing nuget pack for project {0}.{1}{2}",
                        item.Name, Environment.NewLine, result));
                }

                yield return result;
            }
        }

        public RunResult ChocolateyPack(ISolutionContext solutionContext, string chocolateySpecPath)
        {
            var nugetExePath = Path.Combine(solutionContext.Path, "build", "tools", "chocolatey", "choco.exe");
            return _chocolatey.Pack(nugetExePath, chocolateySpecPath);
        }

        private static IEnumerable<Project> GetNugetProjects(ISolutionContext solutionContext)
        {
            return solutionContext.Solution.Projects
                .Where(p => p.NugetSpec != null);
        }
    }
}