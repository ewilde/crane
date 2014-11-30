using System.IO;

namespace Crane.Core.Configuration
{
    public interface IConfiguration
    {
        string BuildFolderName { get; }

        string BuildTemplateProviderName { get; }
    }
}