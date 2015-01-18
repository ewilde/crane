using Crane.Core.Api.Builders;

namespace Crane.Core.Api
{
    public class CraneApi : ICraneApi
    {
        public ISolutionContext GetSolutionContext(string rootFolderPath)
        {
            return new SolutionContext();
        }        
    }
}