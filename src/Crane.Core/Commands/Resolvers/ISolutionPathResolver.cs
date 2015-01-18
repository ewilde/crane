namespace Crane.Core.Commands.Resolvers
{
    public interface ISolutionPathResolver
    {
        string GetPathRelativeFromBuildFolder(string rootFolder, params string[] ignoreDirs);

        string GetPath(string rootFolder);
    }
}