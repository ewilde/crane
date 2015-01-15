using Crane.Core.Api.Builders;

namespace Crane.Core.Api
{
    public static class CraneApi
    {
        public static SolutionContext GetSolutionContext(string rootFolderPath)
        {
            return new SolutionContext();
        }        
    }
}