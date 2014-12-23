namespace Crane.Core.Configuration
{
    public class ProjectContext : IProjectContext
    {
        public string ProjectName { get; set; }
        public string SolutionName { get; set; }
    }
}