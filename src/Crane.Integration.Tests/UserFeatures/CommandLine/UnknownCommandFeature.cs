using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class UnknownCommandFeature
    {
        [Scenario]
        public void Calling_crane_with_a_command_that_does_not_exist_prompts_for_help(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane foobar"
                ._(() => result = run.Command(craneTestContext.Directory, "crane foobar"));

            "Then I receive a message saying 'error: crane foosadf is not a crane command. See 'crane listcommands'"
                ._(() => result.StandardOutput.Should().Be("error: crane foobar is not a crane command. See 'crane listcommands'"))
                .Teardown(() => craneTestContext.TearDown());
        }

    }
}
