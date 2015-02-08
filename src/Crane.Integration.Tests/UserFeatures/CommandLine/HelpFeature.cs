using Crane.Core.Configuration;
using Crane.Core.Extensions;
using Crane.Core.Runners;
using Crane.Tests.Common.Context;
using Crane.Tests.Common.Runners;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class HelpFeature
    {
        [Scenario]
        public void showing_help_for_a_command(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "When I run crane help init"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane help init"));

            "Then crane outputs the usage statement for the command'"
                ._(() => result.StandardOutput.Line(0).Should().Contain("usage: crane init"));

            "And it should output the example usage"
                ._(() => result.StandardOutput.Should().Contain("example 1"));

            "And it should output the more information message"
                ._(() => result.StandardOutput.Should().Contain("For more information, visit"))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void showing_help_for_a_command_that_has_no_arguments(CraneRunner craneRunner, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => craneRunner = new CraneRunner());

            "When I run crane help listcommands"
                ._(() => result = craneRunner.Command(craneTestContext.BuildOutputDirectory, "crane help listcommands"));

            "Then crane outputs the usage statement for the command'"
                ._(() => result.StandardOutput.Line(0).Should().Contain("usage: crane listcommands"))
                .Teardown(() => craneTestContext.TearDown());
        }
    }
}
