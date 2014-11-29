using System.IO;

namespace Crane.Core.Templates
{
    public interface IBuildTemplate : ITemplate
    {
        string BuildScript { get; }
        
        void Create();
    }
}