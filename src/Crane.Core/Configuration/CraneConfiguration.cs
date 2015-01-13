namespace Crane.Core.Configuration
{
    public class CraneConfiguration : IConfiguration
    {
        public const string DefaultBuildProviderName = "Psake";
        public const string DefaultSourceProviderName = "VisualStudio";
        public const string DefaultBuildFolderName = "build";
        public const string DefaultSourceFolderName = "src";

        /// <summary>
        /// Name of the currently configured build template provider
        /// </summary>
        public string BuildTemplateProviderName
        {
            get { return DefaultBuildProviderName; } 
        }

        /// <summary>
        /// Name of the folder containing the build scripts        
        /// </summary>
        public string BuildFolderName
        {
            get
            {
                return DefaultBuildFolderName;
            }
        }

        /// <summary>
        /// Name of the folder containing the projects source code
        /// </summary>
        public string SourceFolderName
        {
            get
            {
                return DefaultSourceFolderName;
            }
        }

        public string SourceTemplateProviderName
        {
            get { return DefaultSourceProviderName; }
        }
    }
}