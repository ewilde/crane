namespace Crane.Core.Commands.Resolvers
{
    public interface IRelativeSolutionPathToBuildFolderResolver
    {
        string ResolveSolutionPath(string rootFolder, params string[] ignoreDirs);
    }
}