namespace Crane.Core.Configuration
{
    public class ProjectContextFactory : IProjectContextFactory
    {
        public IProjectContext Create(string projectName, string solutionPath)
        {
            return new ProjectContext
            {
                ProjectName = projectName,
                SolutionPath = solutionPath
            };
        }
    }
}