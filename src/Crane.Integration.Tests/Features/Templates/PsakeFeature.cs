using System.IO;
using Crane.Core.Configuration;
using Crane.Core.Templates.Psake;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.Features.Templates
{
    public class PsakeFeature
    {
        [Scenario]
        public void creating_a_new_psake_build(DirectoryInfo root, PsakeBuildTemplate template, ICraneContext context)
        {            
            "Given I have a project root folder"
                ._(() =>
                {
                    context = ioc.Resolve<ICraneContext>();
                    context.ProjectRootDirectory = new DirectoryInfo(FileUtilites.GetTemporaryDirectory());
                });

            "And I have a psake template builder"
                ._(() => template = ioc.Resolve<PsakeBuildTemplate>());

            "And I have been given a project name via init"
                ._(() => context.Configuration.ProjectName = "ServiceStack");

            "When I call create on the template"
                ._(() => template.Create());

            "It should replace the solution file name in the build script with the project name"
                ._(() => File.ReadAllText(template.BuildScript.FullName).Should().Contain("ServiceStack.sln"))
                .Teardown(() => Directory.Delete(context.ProjectRootDirectory.FullName, recursive: true));            
        }
    }
}