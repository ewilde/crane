using Crane.Core.Configuration;
using Crane.Integration.Tests.TestUtilities;
using FluentAssertions;
using Xbehave;

namespace Crane.Integration.Tests.UserFeatures.CommandLine
{
    public class ListCommandsFeature
    {
        private const string PossibleCommands =
            @"list of possible crane commands:crane assemble crane help crane init crane listcommands ";

        [Scenario]
        public void Calling_crane_with_no_arguments_will_list_all_possible_commands_except_unknown(Run run,
            RunResult result, CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane"
                ._(() => result = run.Command(craneTestContext.BuildOutputDirectory, "crane"));

            "Then I receive a message containing all of the possible commands listed alphabetically"
                ._(() => result.StandardOutput.Should().Be(PossibleCommands))
                .Teardown(() => craneTestContext.TearDown());
        }

        [Scenario]
        public void Calling_crane_listcommands_will_list_all_possible_commands_except_unknown(Run run, RunResult result,
            CraneTestContext craneTestContext)
        {
            "Given I have my own private copy of the crane console"
                ._(() => craneTestContext = ServiceLocator.Resolve<CraneTestContext>());

            "And I have a run context"
                ._(() => run = new Run());

            "When I run crane"
                ._(() => result = run.Command(craneTestContext.BuildOutputDirectory, "crane listcommands"));

            "Then I receive a message containing all of the possible commands listed alphabetically"
                ._(() => result.StandardOutput.Should().Be(PossibleCommands))
                .Teardown(() => craneTestContext.TearDown());
        }
    }
}
