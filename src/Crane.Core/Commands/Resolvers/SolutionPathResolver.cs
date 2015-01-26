using System.IO;
using System.Linq;
using Crane.Core.Commands.Exceptions;

namespace Crane.Core.Commands.Resolvers
{
    public class SolutionPathResolver : ISolutionPathResolver
    {
        public string GetPathRelativeFromBuildFolder(string rootFolder, params string[] ignoreDirs)
        {
            var root = new DirectoryInfo(rootFolder);
            return GetPath(rootFolder).Replace(root.FullName, "..");
        }

        public string GetPath(string rootFolder)
        {
            var root = new DirectoryInfo(rootFolder);
            var solutions = root.GetFiles("*.sln", SearchOption.AllDirectories);

            if (solutions.Length == 0)
                throw new NoSolutionsFoundCraneException(rootFolder);

            if (solutions.Length > 1)
                throw new MultipleSolutionsFoundCraneException(solutions.Select(s => s.Name).ToArray());

            return solutions.First().FullName;
        }
    }
}