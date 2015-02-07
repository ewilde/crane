using System.Diagnostics;
using System.IO;
using Crane.Core.Configuration;
using Crane.Tests.Common;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using Crane.Tests.Common.FluentExtensions;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class DefaultBuildScriptFeature
    {
        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void build_a_new_default_crane_project_sucessfully(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a crane run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have a new crane project 'SallyFx'"
                ._(() => craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init SallyFx").ErrorOutput.Should().BeEmpty());

            "When I build the project"
                ._(() =>
                {
                    result = new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "SallyFx"));
                    result.ErrorOutput.Should().BeEmpty();
                });

            "It should have build the main 'SallyFx' class library"
                ._(() => File.Exists(Path.Combine(craneTestContext.BuildOutputDirectory, "SallyFx", "build-output", "SallyFx.dll")));

            "It should have build the 'SallyFx' unit test library"
                ._(() => File.Exists(Path.Combine(craneTestContext.BuildOutputDirectory, "SallyFx", "build-output", "SallyFx.UnitTests.dll")));

            "It should have a default assembly version 0.0.0.0 which is done via assembly info patching"
                ._(() => FileVersionInfo.GetVersionInfo(Path.Combine(craneTestContext.BuildOutputDirectory, "SallyFx", "build-output", "SallyFx.dll"))
                    .FileVersion.Should().Be("0.0.0.0"));

            "It should build successfully"
                ._(() => result.StandardOutput.Should().Contain("Succeeded!"));

            "It should not throw an error"
                ._(() => result.ErrorOutput.Should().BeEmpty())
                .Teardown(() => craneTestContext.TearDown());
        }

        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void building_a_default_crane_project_that_is_also_a_git_repo(CraneRunner craneRunner, RunResult result,
            CraneTestContext craneTestContext, Git git)
        {
            string projectDir = null;
            "Given I have my own private copy of the crane console"
               ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a crane run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have a new crane project 'SallyFx'"
                ._(() => craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init SallyFx").ErrorOutput.Should().BeEmpty());

            "And I initialize that as a git repository"
                ._(() =>
                {
                    projectDir = Path.Combine(craneTestContext.BuildOutputDirectory, "SallyFx");
                    git = ServiceLocator.Resolve<Git>();
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
                   result = new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "SallyFx"));
                   result.ErrorOutput.Should().BeEmpty();
               });

            "It should have the commit message as part of the additional file information"
               ._(() =>
               {
                   var fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(craneTestContext.BuildOutputDirectory, "SallyFx", "build-output", "SallyFx.dll"));
                   fileVersionInfo.ProductVersion.Should().Contain("First commit of SallyFx");
               })
               .Teardown(() => craneTestContext.TearDown());
        }

        [ScenarioIgnoreOnMono("Powershell not fully supported on mono")]
        public void build_a_project_and_publish_to_nuget(
            NuGetServerContext nuGetServer, 
            ICraneTestContext craneTestContext, 
            CraneRunner craneRunner,
            RunResult result)
        {
            "Given I have my own private copy of the crane console"
             ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a nuget server running"
                ._(() => nuGetServer = new NuGetServerContext(craneTestContext))
                .Teardown(() =>
                {
                    nuGetServer.TearDown();
                    craneTestContext.TearDown();
                });

            "And I have a project with a nuget spec file (which is the default behaviour of crane init)"
                ._(() => craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init SallyFx").ErrorOutput.Should().BeEmpty());

            "When I build the project supplying the nuget details"
               ._(() =>
               {
                   result = new BuildScriptRunner().Run(Path.Combine(craneTestContext.BuildOutputDirectory, "SallyFx"));
                   result.Should().BeBuiltSuccessfulyWithAllTestsPassing().And.BeErrorFree();
               });



        }
    }
}