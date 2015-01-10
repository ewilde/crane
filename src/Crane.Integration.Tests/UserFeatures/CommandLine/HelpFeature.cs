using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crane.Core.Extensions;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class HelpFeature
    {
        [Scenario]
        public void showing_help_for_a_command(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane help init"
                ._(() => result = run.Command(craneTestContext.BuildOutputDirectory, "crane help init"));

            "Then crane outputs the usage statement for the command'"
                ._(() => result.StandardOutput.Line(0).Should().Contain("usage: crane init"));

            "And it should output the example usage"
                ._(() => result.StandardOutput.Should().Contain("example 1"));

            "And it should output the more information message"
                ._(() => result.StandardOutput.Should().Contain("For more information, visit"))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void showing_help_for_a_command_that_has_no_arguments(Run run, RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ioc.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane help listcommands"
                ._(() => result = run.Command(craneTestContext.BuildOutputDirectory, "crane help listcommands"));

            "Then crane outputs the usage statement for the command'"
                ._(() => result.StandardOutput.Line(0).Should().Contain("usage: crane listcommands"))
                .Teardown(() => craneTestContext.TearDown());
        }
    }
}
