using System.IO;
using Crane.Core.Configuration;
using Crane.Core.Utility;
using FakeItEasy;

namespace Crane.Core.Tests.TestExtensions
{
    public static class DefaultConfigurationUtility
    {
        public static void PostInit(
            IConfiguration configuration, 
            string projectName = null, 
            string buildProviderName = null, 
            string buildFolderName = null)
        {
            if (buildFolderName == null)
            {
                buildFolderName = CraneConfiguration.DefaultBuildFolderName;
            }

            A.CallTo(() => configuration.BuildTemplateProviderName).Returns(buildProviderName ?? CraneConfiguration.DefaultTemplateProviderName);
            A.CallTo(() => configuration.BuildFolderName).Returns(buildFolderName);
            A.CallTo(() => configuration.ProjectName).Returns(projectName ?? StringExtensions.RandomString(8));
        }
    }
}