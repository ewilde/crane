using System.IO;
using Crane.Core.Configuration;
using Crane.Core.Extensions;
using FakeItEasy;

namespace Crane.Core.Tests.TestUtilities
{
    public static class ContextUtility
    {
        public static void Configure(
            ICraneContext context, 
            IConfiguration configuration = null,             
            DirectoryInfo projectRootDirectory = null,
            DirectoryInfo craneInstallDirectory = null,
            DirectoryInfo buildDirectory = null,
            DirectoryInfo sourceDirectory = null)
        {            
            A.CallTo(() => context.ProjectRootDirectory).Returns(projectRootDirectory ?? new DirectoryInfo(Path.Combine(@"C:\", StringExtensions.RandomString(8))));
            A.CallTo(() => context.BuildDirectory).Returns(buildDirectory ?? new DirectoryInfo(Path.Combine(context.ProjectRootDirectory.FullName, CraneConfiguration.DefaultBuildFolderName)));            
            A.CallTo(() => context.CraneInstallDirectory).Returns(craneInstallDirectory ?? new DirectoryInfo(CraneContext.DefaultInstallationDirectory));
            A.CallTo(() => context.SourceDirectory).Returns(sourceDirectory ?? new DirectoryInfo(Path.Combine(context.ProjectRootDirectory.FullName, CraneConfiguration.DefaultSourceFolderName)));
        }
    }
}