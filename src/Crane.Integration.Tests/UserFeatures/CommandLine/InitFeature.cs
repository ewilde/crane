using System.IO;
using Crane.Core.Configuration;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class InitFeature
    {      
        [Scenario]
        public void Init_with_no_arguments_returns_did_you_mean_init_projectname(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "When I run crane init"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init"));

            "Then I receive an error message 'error: The argument 'ProjectName' is required'"
                ._(() => result.StandardOutput.Should().Contain("error: The argument 'ProjectName' is required"))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void Init_with_a_project_name_creates_a_project_with_build(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "When I run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "It should say 'Init success.'"
                ._(() => result.StandardOutput.Should().Be("Init success."));

            "It should replace the solution file name in the build script with the project name"
                ._(() => File.ReadAllText(Path.Combine(craneTestContext.BuildOutputDirectory, "ServiceStack", "build", "default.ps1")).Should().Contain("ServiceStack.sln"))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void Init_with_a_project_name_twice_gives_error(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "And I have run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "When I run crane init ServiceStack"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane init ServiceStack"));

            "It should give an error'"
                ._(() => result.StandardOutput.Should().Contain("ServiceStack"));

            "It should not have an exit code of 0"
                ._(() => result.ExitCode.Should().NotBe(0))
                .Teardown(() => craneTestContext.TearDown());
        }       
    }
}
