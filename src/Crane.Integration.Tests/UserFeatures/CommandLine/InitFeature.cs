using System.IO;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class InitFeature
    {      
        [Scenario]
        public void Init_with_no_arguments_returns_did_you_mean_init_projectname(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane init"
                ._(() => result = run.Command(craneTestContext.Directory, "crane init"));

            "Then I receive the text did you mean 'crane init projectname'?"
                ._(() => result.StandardOutput.Should().Be("did you mean 'crane init projectname'?"))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void Init_with_a_project_name_creates_a_project(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane init ServiceStack"
                ._(() => result = run.Command(craneTestContext.Directory, "crane init ServiceStack"));

            "It should say 'Init success.'"
                ._(() => result.StandardOutput.Should().Be("Init success."));

            "It should replace the solution file name in the build script with the project name"
                ._(() => File.ReadAllText(Path.Combine(craneTestContext.Directory, @"ServiceStack\build\default.ps1")).Should().Contain("ServiceStack.sln"))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void Init_with_a_project_name_twice_gives_error(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "And I have run crane init ServiceStack"
                ._(() => result = run.Command(craneTestContext.Directory, "crane init ServiceStack"));

            "When I run crane init ServiceStack"
                ._(() => result = run.Command(craneTestContext.Directory, "crane init ServiceStack"));

            "It should give an error'"
                ._(() => result.StandardOutput.Should().Contain("ServiceStack"));

            "It should not have an exit code of 0"
                ._(() => result.ExitCode.Should().NotBe(0))
                .Teardown(() => craneTestContext.TearDown());
        }
        
    }
}
