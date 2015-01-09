namespace Crane.Core.Configuration
{
    public interface IProjectContextFactory
    {
        IProjectContext Create(string projectName, string solutionPath);
    }
}