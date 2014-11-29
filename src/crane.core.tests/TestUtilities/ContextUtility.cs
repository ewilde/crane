using System;
using System.IO;
using Crane.Core.Configuration;
using Crane.Core.Utility;
using FakeItEasy;

namespace Crane.Core.Tests.TestExtensions
{
    public static class ContextUtility
    {
        public static void Configure(
            ICraneContext context, 
            IConfiguration configuration = null, 
            DirectoryInfo projectRootDirectory = null,
            DirectoryInfo craneInstallDirectory = null,
            DirectoryInfo buildDirectory = null)
        {
            A.CallTo(() => context.ProjectRootDirectory).Returns(projectRootDirectory ?? new DirectoryInfo(Path.Combine(@"C:\", StringExtensions.RandomString(8))));
            A.CallTo(() => context.BuildDirectory).Returns(buildDirectory ?? new DirectoryInfo(Path.Combine(context.ProjectRootDirectory.FullName, CraneConfiguration.DefaultBuildFolderName)));            
            A.CallTo(() => context.CraneInstallDirectory).Returns(craneInstallDirectory ?? new DirectoryInfo(CraneContext.DefaultInstallationDirectory));
        }
    }
}