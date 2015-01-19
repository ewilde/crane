using System;
using System.IO;
using Crane.Core.Api.Builders;
using Crane.Core.Api.Readers;
using Crane.Core.Commands.Resolvers;

namespace Crane.Core.Api
{
    public class CraneApi : ICraneApi
    {
        private readonly ISolutionReader _solutionReader;
        private readonly Func<ISolutionContext> _solutionContext;
        private readonly ISolutionPathResolver _solutionPathResolver;

        public CraneApi(ISolutionReader solutionReader, Func<ISolutionContext> solutionContext, ISolutionPathResolver solutionPathResolver)
        {
            _solutionReader = solutionReader;
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

        public ISolutionContext PatchAssemblyInfo(Project project)
        {
            return GetSolutionContext(project.Solution.SolutionContext.Path);
        }

        private string GetRelativePathToSolution(string rootFolderPath)
        {
            return _solutionPathResolver.GetPath(rootFolderPath);
        }
    }
}