using System.IO;

namespace Crane.Core.Templates
{
    public interface IBuildTemplate : ITemplate
    {
        FileInfo BuildScript { get; }
        
        void Create();
    }
}