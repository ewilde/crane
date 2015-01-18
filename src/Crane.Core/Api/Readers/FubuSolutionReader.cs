using System.IO;
using System.Linq;

namespace Crane.Core.Api.Readers
{
    public class FubuSolutionReader : ISolutionReader
    {
        public Solution FromPath(string path)
        {
            var fubuSolution = FubuCsProjFile.Solution.LoadFrom(path);

            var projects = fubuSolution.Projects.Select(item => new Project
            {
                Name = item.ProjectName,
                Path = Path.GetFullPath(item.Project.FileName)
            });

            var solution = new Solution
            {
                Name = fubuSolution.Name,
                Path = fubuSolution.Filename,
                Projects = projects
            };

            return solution;
        }
    }
}