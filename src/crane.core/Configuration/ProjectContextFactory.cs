namespace Crane.Core.Configuration
{
    public class ProjectContextFactory : IProjectContextFactory
    {
        public IProjectContext Create(string projectName, string solutionName)
        {
            return new ProjectContext
            {
                ProjectName = projectName,
                SolutionName = solutionName
            };
        }
    }
}