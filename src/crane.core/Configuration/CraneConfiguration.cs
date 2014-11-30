using System;
using System.IO;

namespace Crane.Core.Configuration
{
    public class CraneConfiguration : IConfiguration
    {
        public const string DefaultTemplateProviderName = "Psake";
        public const string DefaultBuildFolderName = "build";

        public string BuildTemplateProviderName
        {
            get { return DefaultTemplateProviderName; } 
        }

        public string BuildFolderName
        {
            get
            {
                return DefaultBuildFolderName;
            }
        }
    }
}