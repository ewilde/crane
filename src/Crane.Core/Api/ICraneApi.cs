namespace Crane.Core.Api
{
    public interface ICraneApi
    {
        ISolutionContext GetSolutionContext(string rootFolderPath);
    }
}