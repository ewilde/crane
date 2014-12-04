using System.IO;
using Crane.Core.Configuration;
using Crane.Core.IO;
using Crane.Core.Templates.Psake;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Templates
{
    public class PsakeBuildTemplateFeature
    {
        [Scenario]
        public void creating_a_new_psake_build(DirectoryInfo root, PsakeBuildTemplate template, ICraneContext context, IFileManager fileManager)
        {            
            "Given I have a project root folder"
                ._(() =>
                {
                    context = ioc.Resolve<ICraneContext>(); fileManager = ioc.Resolve<IFileManager>();
                    context.ProjectRootDirectory = new DirectoryInfo(fileManager.GetTemporaryDirectory());
                });

            "And I have a psake template builder"
                ._(() => template = ioc.Resolve<PsakeBuildTemplate>());

            "And I have been given a project name via init"
                ._(() => context.ProjectName = "ServiceStack");

            "When I call create on the template"
                ._(() => template.Create());

            "It place a build.ps1 in the root project directory"
                ._(() => File.Exists(Path.Combine(context.ProjectRootDirectory.FullName, "build.ps1")).Should().BeTrue("build.ps1 should be in root directory"));

            "It should replace the solution file name in the build script with the project name"
                ._(() => File.ReadAllText(template.BuildScript.FullName).Should().Contain("ServiceStack.sln"))
                .Teardown(() => Directory.Delete(context.ProjectRootDirectory.FullName, recursive: true));            
        }
    }
}