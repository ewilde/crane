using Crane.Core.Configuration;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class UnknownCommandFeature
    {
        [Scenario]
        public void Calling_crane_with_a_command_that_does_not_exist_prompts_for_help(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a crane run context"
                ._(() => craneRunner = new CraneRunner());

            "When I run crane foobar"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane foobar"));

            "Then I receive a message saying 'error: crane foosadf is not a crane command. See 'crane listcommands'"
                ._(() => result.StandardOutput.Should().Be("error: crane foobar is not a crane command. See 'crane listcommands'"))
                .Teardown(() => craneTestContext.TearDown());
        }

    }
}
