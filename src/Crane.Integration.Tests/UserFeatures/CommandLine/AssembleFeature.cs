using System.IO;
using System.Linq;
using Crane.Integration.Tests.TestUtilities;
using Crane.Integration.Tests.TestUtilities.Extensions;
using FluentAssertions;
using Ionic.Zip;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{

    public class AssembleFeature
    {        
        [ScenarioIgnoreOnMonoAttribute("Powershell not fully supported on mono")]
        public void Assemble_with_a_folder_name_creates_a_project_with_build_when_folder_name_matches_solution_name(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "And I have a project called ServiceStack with no build"
                ._(() =>
                {
                    File.Copy("./TestProjects/ProjectNameSameAsSolution.zip", Path.Combine(craneTestContext.Directory, "ProjectNameSameAsSolution.zip"), true);
                    var zipFile = ZipFile.Read(Path.Combine(craneTestContext.Directory, "ProjectNameSameAsSolution.zip"));
                    zipFile.ExtractAll(craneTestContext.Directory, ExtractExistingFileAction.OverwriteSilently);
                });

            "When I run crane assemble ServiceStack"
                ._(() => result = run.Command(craneTestContext.Directory, "crane Assemble ServiceStack"));

            "It should say 'Assemble success.'"
                ._(() =>
                {
                    result.ErrorOutput.Should().BeEmpty();
                    result.StandardOutput.Should().Be("Assemble success.");
                });

            "It should create a build.ps1 in the top level folder"
                ._(
                    () => new DirectoryInfo(Path.Combine(craneTestContext.Directory, "ServiceStack")).GetFiles()
                            .Count(f => f.Name.ToLower() == "build.ps1")
                            .Should()
                            .Be(1)
                );

            "It should be able to be built"
                ._(() =>
                {
                    var buildResult = new BuildScriptRunner().Run(Path.Combine(craneTestContext.Directory, "ServiceStack"));
                    buildResult.Should().BeBuildSuccessful().And.BeErrorFree();

                });

            "It should create a build for the project with a reference to the solution file"
				._(() => File.ReadAllText(Path.Combine(craneTestContext.Directory, "ServiceStack" , "build", "default.ps1")).Should().Contain("ServiceStack.sln"))
                .Teardown(() => craneTestContext.TearDown());
        }
    }

}
