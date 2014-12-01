using System.IO;

namespace Crane.Core.Configuration
{
    public interface IConfiguration
    {
        string BuildFolderName { get; }

        string BuildTemplateProviderName { get; }

        /// <summary>
        /// Name of the folder containing the projects source code
        /// </summary>
        string SourceFolderName { get; }
    }
}