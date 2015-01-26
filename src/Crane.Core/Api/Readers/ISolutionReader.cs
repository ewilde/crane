using Crane.Core.Api.Model;

namespace Crane.Core.Api.Readers
{
    public interface ISolutionReader
    {
        Solution FromPath(string path);
    }
}