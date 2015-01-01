using System.Diagnostics;
using System.IO;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class DefaultBuildScriptFeature
    {
		[ScenarioIgnoreOnMonoAttribute("Powershell not fully supported on mono")]
        public void build_a_new_default_crane_project_sucessfully(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a crane run context"
                ._(() => run = new Run());

            "And I have a new crane project 'SallyFx'"
                ._(() => run.Command(craneTestContext.Directory, "crane init SallyFx").ErrorOutput.Should().BeEmpty());

            "When I build the project"
                ._(() =>
                {
                    result = new BuildScriptRunner().Run(Path.Combine(craneTestContext.Directory, "SallyFx"));
                    result.ErrorOutput.Should().BeEmpty();
                });

            "It should have build the main 'SallyFx' class library"
                ._(() => File.Exists(Path.Combine(craneTestContext.Directory, "SallyFx", "build-output", "SallyFx.dll")));

            "It should have build the 'SallyFx' unit test library"
                ._(() => File.Exists(Path.Combine(craneTestContext.Directory, "SallyFx", "build-output", "SallyFx.UnitTests.dll")));

            "It should have a default assembly version 0.0.0.0 which is done via assembly info patching"
                ._(() => FileVersionInfo.GetVersionInfo(Path.Combine(craneTestContext.Directory, "SallyFx", "build-output", "SallyFx.dll"))
                    .FileVersion.Should().Be("0.0.0.0"));

            "It should not throw an error"
                ._(() => result.ErrorOutput.Should().BeEmpty())
                .Teardown(() => craneTestContext.TearDown()); 
        }

		[ScenarioIgnoreOnMonoAttribute("Powershell not fully supported on mono")]
		public void building_a_default_crane_project_that_is_also_a_git_repo(Run run, RunResult result,
            CraneTestContext craneTestContext, Git git)
        {
            string projectDir = null;
            "Given I have my own private copy of the crane console"
               ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a crane run context"
                ._(() => run = new Run());

            "And I have a new crane project 'SallyFx'"
                ._(() => run.Command(craneTestContext.Directory, "crane init SallyFx").ErrorOutput.Should().BeEmpty());

            "And I initialize that as a git repository"
                ._(() =>
                {
                    projectDir = Path.Combine(craneTestContext.Directory, "SallyFx");
                    git = ioc.Resolve<Git>();
                    git.Run("init", projectDir).ErrorOutput.Should().BeEmpty();
                    git.Run("config user.email no-reply@cranebuild.com", projectDir).ErrorOutput.Should().BeEmpty();
                });

            "And I have a previous commit"
                ._(() =>
                {
                    git.Run("add -A", projectDir).ErrorOutput.Should().BeEmpty();
                    git.Run("config user.email no-reply@cranebuild.com", projectDir).ErrorOutput.Should().BeEmpty();
                    git.Run("commit -m \"First commit of SallyFx\"", projectDir).ErrorOutput.Should().BeEmpty();
                });

            "When I build the project"
               ._(() =>
               {
                   result = new BuildScriptRunner().Run(Path.Combine(craneTestContext.Directory, "SallyFx"));
                   result.ErrorOutput.Should().BeEmpty();
               });

            "It should have the commit message as part of the additional file information"
               ._(() =>
               {
                   var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(craneTestContext.Directory, "SallyFx", "build-output", "SallyFx.dll"));
                   fileVersionInfo.ProductVersion.Should().Contain("First commit of SallyFx");
               })
               .Teardown(() => craneTestContext.TearDown()); 
        }
    }
}