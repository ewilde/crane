namespace Crane.Core.Configuration
{
    public interface IProjectContext
    {
        string ProjectName { get; set; }
        string SolutionPath { get; set; }
    }
    
}