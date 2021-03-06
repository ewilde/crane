﻿using Crane.Core.Configuration;
using FakeItEasy;

namespace Crane.Core.Tests.TestUtilities
{
    public static class DefaultConfigurationUtility
    {
        public static void PostInit(
            IConfiguration configuration, 
            string buildProviderName = null, 
            string buildFolderName = null,
            string sourceProviderName = null,
            string sourceFolderName = null)
        {
            if (buildFolderName == null)
            {
                buildFolderName = CraneConfiguration.DefaultBuildFolderName;
            }

            A.CallTo(() => configuration.BuildTemplateProviderName).Returns(buildProviderName ?? CraneConfiguration.DefaultBuildProviderName);
            A.CallTo(() => configuration.SourceTemplateProviderName).Returns(sourceProviderName ?? CraneConfiguration.DefaultSourceProviderName);
            A.CallTo(() => configuration.BuildFolderName).Returns(buildFolderName);
            A.CallTo(() => configuration.SourceFolderName).Returns(sourceFolderName ?? CraneConfiguration.DefaultSourceFolderName);
        }
    }
}