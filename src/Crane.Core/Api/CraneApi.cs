using System;
using System.IO;
using Crane.Core.Api.Model;
using Crane.Core.Api.Readers;
using Crane.Core.Api.Writers;
using Crane.Core.Commands.Resolvers;

namespace Crane.Core.Api
{
    public class CraneApi : ICraneApi
    {
        private readonly ISolutionReader _solutionReader;
        private readonly IAssemblyInfoWriter _assemblyInfoWriter;
        private readonly Func<ISolutionContext> _solutionContext;
        private readonly ISolutionPathResolver _solutionPathResolver;

        public CraneApi(
            ISolutionReader solutionReader,
            IAssemblyInfoWriter assemblyInfoWriter,
            Func<ISolutionContext> solutionContext, ISolutionPathResolver solutionPathResolver)
        {
            _solutionReader = solutionReader;
            _assemblyInfoWriter = assemblyInfoWriter;
            _solutionContext = solutionContext;
            _solutionPathResolver = solutionPathResolver;
        }

        public ISolutionContext GetSolutionContext(string rootFolderPath)
        {
            var context = _solutionContext();
            context.Path = rootFolderPath;
            context.Solution = _solutionReader.FromPath(Path.Combine(rootFolderPath, GetRelativePathToSolution(rootFolderPath)));
            context.Solution.SolutionContext = context;
            return context;
        }

        public void PatchAssemblyInfo(AssemblyInfo assemblyInfo)
        {
            _assemblyInfoWriter.Patch(assemblyInfo);            
        }

        private string GetRelativePathToSolution(string rootFolderPath)
        {
            return _solutionPathResolver.GetPath(rootFolderPath);
        }
    }
}